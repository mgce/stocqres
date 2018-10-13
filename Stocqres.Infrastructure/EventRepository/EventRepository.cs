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
            var eventsToSave = events.Select(e => e.ToEventData(aggregate.Id, aggregateType, originalVersion++));
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var tx = conn.BeginTransaction())
                {
                    await CreateTableForAggregateIfNotExist(conn, aggregateType);
                    var foundVersionQuery = $"Select MAX(Version) FROM Events WHERE AggregateId={aggregate.Id}";
                    var foundVersionCommand = new SqlCommand(foundVersionQuery, conn);
                    var foundVersionResult = (int?) foundVersionCommand.ExecuteScalar();

                    if(foundVersionResult.HasValue && foundVersionResult >= originalVersion)
                        throw new Exception("Concurrency Exception");

                    const string sql =
                        @"INSERT INTO Events(Id, AggregateId, AggregateType, Version, Data, Metadata, Created) " +
                        @"VALUES(@Id, @AggregateId, @AggregateType, @Version, @Data, @Metadata,@Created)";

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

        private async Task CreateTableForAggregateIfNotExist(SqlConnection con, string aggregateTableName)
        {
            var sql = $"IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customers].[{aggregateTableName}Events]' AND type in (N'U'))" +
                      "BEGIN" +
                      $"CREATE TABLE [Customers].[{aggregateTableName}Events](" +
                      "[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL" +
                      "[AggregateId] UNIQUEIDENTIFIER default NEWID() NOT NULL" +
                      "[AggregateType] NVARCHAR(255) NOT NULL" +
                      "[Version] INT NOT NULL" +
                      "[Data] NVARCHAR(MAX) NOT NULL" +
                      "[MetaData] NVARCHAR(MAX) NOT NULL" +
                      "[CreatedAt] DATETIME NOT NULL," +
                      $"CONSTRAINT PK{aggregateTableName}Events PRIMARY KEY(ID)" +
                      ")" +
                      "GO" +
                      $"CREATE INDEX Idx_{aggregateTableName}Events_AggregateId" +
                      $"ON {aggregateTableName}Events(AggregateId)" +
                      "GO" +
                      "END";

            await con.ExecuteScalarAsync<bool>(sql);
        }
    }
}
