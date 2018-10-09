using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly IAggregateRootFactory _factory;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EventRepository(IAggregateRootFactory factory, IEventBus eventBus, IConfiguration configuration)
        {
            _factory = factory;
            _eventBus = eventBus;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var sql = $"Select * From Events Where AggregateId={id}";
                var listOfEventData = await conn.QueryAsync<EventData>(sql, new {id});
                var events = listOfEventData.Select(x => x.DeserializeEvent());
                return await _factory.CreateAsync<T>(events);
            }
        }

        public async Task SaveAsync(IAggregateRoot aggregate)
        {
            var events = aggregate.GetUncommitedEvents();
            if (!events.Any())
                return;
            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count + 1;
            var eventsToSave = events.Select(e => e.ToEventData(aggregate.Id, aggregateType, originalVersion++));
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var tx = conn.BeginTransaction())
                {
                    var foundVersionQuery = $"Select MAX(Version) FROM Events WHERE AggregateId={aggregate.Id}";
                    var foundVersionCommand = new SqlCommand(foundVersionQuery, conn);
                    var foundVersionResult = (int?) foundVersionCommand.ExecuteScalar();

                    if(foundVersionResult.HasValue && foundVersionResult >= originalVersion)
                        throw new Exception("Concurrency Exception");

                    const string sql =
                        @"INSERT INTO Events(AggregateId, Created,  AggregateId, AggregateType, Version, Data, Metadata) " +
                        @"VALUES(@AggregateId, @Created,  @AggregateId, @AggregateType, @Version, @Data, @Metadata)";

                    await conn.ExecuteAsync(sql, eventsToSave, tx);
                    tx.Commit();
                }
            }

            await RaiseEvents(events);
        }

        private async Task RaiseEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                await _eventBus.Publish(@event);
            }
        }
    }
}
