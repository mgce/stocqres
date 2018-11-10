using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.EventRepository.Scripts;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly IAggregateRootFactory _factory;
        private readonly IEventBus _eventBus;
        private readonly string _connectionString;

        public EventRepository(IAggregateRootFactory factory, IEventBus eventBus, IConfiguration configuration)
        {
            _factory = factory;
            _eventBus = eventBus;
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var sql = EventRepositoryScriptsAsStrings.GetAggregate(typeof(T).Name, id);
                var listOfEventData = await conn.QueryAsync<EventData>(sql, new {id});
                var events = listOfEventData.OrderBy(e=>e.Version).Select(x => x.DeserializeEvent());
                return (T)_factory.CreateAsync<T>(events);
            }
        }

        public async Task SaveAsync(IAggregateRoot aggregate)
        {
            var events = aggregate.GetUncommitedEvents();
            if (!events.Any())
                return;
            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count + 1;
            var eventsToSave = events.Select(e => e.ToEventData(aggregate.Id, aggregateType, originalVersion++, DateTime.Now));
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var tx = conn.BeginTransaction())
                    {
                        await CreateTableForAggregateIfNotExist(conn, tx, aggregateType);
                        await CheckAggregateVersion(conn, aggregateType, aggregate.Id, originalVersion);

                        string insertScript = EventRepositoryScriptsAsStrings.InsertIntoAggregate(aggregateType);

                        await conn.ExecuteAsync(insertScript, eventsToSave, tx);
                        tx.Commit();
                    }
                }
                scope.Complete();
            }

            await RaiseEvents(aggregate);
        }

        private async Task RaiseEvents(IAggregateRoot aggregate)
        {
            foreach (var @event in aggregate.GetUncommitedEvents())
            {
                await _eventBus.Publish(@event);
            }
        }

        private async Task CreateTableForAggregateIfNotExist(SqlConnection con, SqlTransaction tx, string aggregateTableName)
        {
            var sql = EventRepositoryScriptsAsStrings.CreateTableForAggregate(aggregateTableName);
            await con.ExecuteAsync(sql,null,tx);

            var indexSql = EventRepositoryScriptsAsStrings.CreateIndex(aggregateTableName);
            await con.ExecuteAsync(indexSql,null,tx);
        }

        private async Task CheckAggregateVersion(SqlConnection conn, string aggregateType, Guid aggregateId, int originalVersion)
        {
            var foundVersionQuery =
                EventRepositoryScriptsAsStrings.FindAggregateVersion(aggregateType, aggregateId);
            var foundVersionResult = await conn.ExecuteScalarAsync(foundVersionQuery);
            if ((int?)foundVersionResult >= originalVersion)
                throw new Exception("Concurrency Exception");
        }
    }
}
