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
                var sql = $"Select * From [Customers].{typeof(T).Name}Events Where AggregateId='{id}'";
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
            var eventsToSave = events.Select(e => e.ToEventData(aggregate.Id, aggregateType, originalVersion++, DateTime.Now));
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var tx = conn.BeginTransaction())
                    {
                        await CreateTableForAggregateIfNotExist(conn, tx, aggregateType);
                        var foundVersionQuery = $"Select MAX(Version) FROM [Customers].{aggregateType}Events WHERE AggregateId='{aggregate.Id}'";
                        var foundVersionResult = (int?)conn.ExecuteScalar(foundVersionQuery);
                        if(foundVersionResult.HasValue && foundVersionResult >= originalVersion)
                            throw new Exception("Concurrency Exception");

                        string sql = $"INSERT INTO [Customers].{aggregateType}Events(Id, AggregateId, AggregateType, Version, Data, Metadata, CreatedAt) " +
                                     "VALUES(@Id, @AggregateId, @AggregateType, @Version, @Data, @Metadata, @Created)";

                        await conn.ExecuteAsync(sql, eventsToSave, tx);
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
            var sql =
                $"IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = '{aggregateTableName}Events' AND xtype='U') " +
                $"CREATE TABLE [Customers].[{aggregateTableName}Events](" +
                "[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL, " +
                "[AggregateId] UNIQUEIDENTIFIER NOT NULL, " +
                "[AggregateType] NVARCHAR(255) NOT NULL, " +
                "[Version] INT NOT NULL, " +
                "[Data] NVARCHAR(MAX) NOT NULL, " +
                "[MetaData] NVARCHAR(MAX) NOT NULL, " +
                "[CreatedAt] DATETIME NOT NULL, " +
                $"CONSTRAINT PK{aggregateTableName}Events PRIMARY KEY(ID) " +
                ") ";
            con.Execute(sql,null,tx);
            
            var indexSql =
                $"IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_{aggregateTableName}Events_AggregateId' AND object_id = OBJECT_ID('[Customers].{aggregateTableName}Events')) " +
                $"begin " +
                $"CREATE INDEX Idx_{aggregateTableName}Events_AggregateId ON [Customers].{aggregateTableName}Events(AggregateId)" +
                $"end";

            await con.ExecuteAsync(indexSql,null,tx);
        }
    }
}
