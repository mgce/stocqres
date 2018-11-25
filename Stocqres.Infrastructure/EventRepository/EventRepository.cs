using System;
using System.Collections.Generic;
using System.Data;
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
using Stocqres.Infrastructure.UnitOfWork;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly IAggregateRootFactory _factory;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;
        private IDbConnection _connection => _unitOfWork.Connection;
        private IDbTransaction _transaction => _unitOfWork.Transaction;
        private readonly string _connectionString;

        public EventRepository(IAggregateRootFactory factory, IEventBus eventBus, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _factory = factory;
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var sql = EventRepositoryScriptsAsStrings.GetAggregate(typeof(T).Name, id);
            var listOfEventData = await _connection.QueryAsync<EventData>(sql, new {id});
            var events = listOfEventData.OrderBy(e=>e.Version).Select(x => x.DeserializeEvent());
            return (T)_factory.CreateAsync<T>(events);
        }

        public async Task SaveAsync(IAggregateRoot aggregate)
        {
            var events = aggregate.GetUncommitedEvents();
            if (!events.Any())
                return;

            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count + 1;
            var eventsToSave = events.Select(e => e.ToEventData(aggregate.Id, aggregateType, originalVersion++, DateTime.Now));

            await CreateTableForAggregateIfNotExist(aggregateType);

            await CheckAggregateVersion(aggregateType, aggregate.Id, originalVersion);

            string insertScript = EventRepositoryScriptsAsStrings.InsertIntoAggregate(aggregateType);

            await _connection.ExecuteAsync(insertScript, eventsToSave, _transaction);

            await RaiseEvents(aggregate);
        }

        private async Task RaiseEvents(IAggregateRoot aggregate)
        {
            foreach (var @event in aggregate.GetUncommitedEvents())
            {
                await _eventBus.Publish(@event);
            }
        }

        private async Task CreateTableForAggregateIfNotExist(string aggregateTableName)
        {
            var sql = EventRepositoryScriptsAsStrings.CreateTableForAggregate(aggregateTableName);
            await _connection.ExecuteAsync(sql,null,_transaction);

            var indexSql = EventRepositoryScriptsAsStrings.CreateIndex(aggregateTableName);
            await _connection.ExecuteAsync(indexSql,null, _transaction);
        }

        private async Task CheckAggregateVersion(string aggregateType, Guid aggregateId, int originalVersion)
        {
            var foundVersionQuery =
                EventRepositoryScriptsAsStrings.FindAggregateVersion(aggregateType, aggregateId);
            var foundVersionResult = await _connection.ExecuteScalarAsync(foundVersionQuery,null,_transaction);
            if ((int?)foundVersionResult >= originalVersion)
                throw new Exception("Concurrency Exception");
        }
    }
}
