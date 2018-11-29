using System;
using System.Linq;
using System.Threading.Tasks;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.EventSourcing;
using Stocqres.Core.Exceptions;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository.Scripts;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly IAggregateRootFactory _factory;
        private readonly IEventBus _eventBus;
        private readonly IDatabaseProvider _databaseProvider;

        public EventRepository(IAggregateRootFactory factory, IEventBus eventBus, IDatabaseProvider databaseProvider)
        {
            _factory = factory;
            _eventBus = eventBus;
            _databaseProvider = databaseProvider;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            if(id == Guid.Empty)
                throw new StocqresException("Aggregate Id cannot be empty");

            var snapshot = await GetSnapshotIfExist(id);
            if (snapshot != null)
                return await RecreateAggregateFromSnapshot<T>(snapshot);

            var getAggregateSql = EventRepositoryScriptsAsStrings.GetAggregate(typeof(T).Name, id);
            var listOfEventData = await _databaseProvider.QueryAsync<EventData>(getAggregateSql, new {id});

            if(listOfEventData == null || !listOfEventData.Any())
                throw new StocqresException("Aggregate doesn't exist");

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

            //await CreateTableForAggregateIfNotExist(aggregateType);

            await CheckAggregateVersion(aggregateType, aggregate.Id, originalVersion);

            string insertScript = EventRepositoryScriptsAsStrings.InsertIntoAggregate(aggregateType);

            await _databaseProvider.ExecuteAsync(insertScript, eventsToSave);

            await RaiseEvents(aggregate);
        }

        public async Task TakeSnapshot<T>(Guid aggregateId) where T : IAggregateRoot
        {
            var aggregate = await GetByIdAsync<T>(aggregateId);

            var snapshot = aggregate.ToSnapshot();

            string insertScript = EventRepositoryScriptsAsStrings.InsertSnapshot();

            await _databaseProvider.ExecuteAsync(insertScript, snapshot);
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
            await _databaseProvider.ExecuteAsync(sql);

            var indexSql = EventRepositoryScriptsAsStrings.CreateIndex(aggregateTableName);
            await _databaseProvider.ExecuteAsync(indexSql);
        }

        private async Task CheckAggregateVersion(string aggregateType, Guid aggregateId, int originalVersion)
        {
            var foundVersionQuery =
                EventRepositoryScriptsAsStrings.FindAggregateVersion(aggregateType, aggregateId);
            var foundVersionResult = await _databaseProvider.ExecuteScalarAsync(foundVersionQuery);
            if ((int?)foundVersionResult >= originalVersion)
                throw new Exception("Concurrency Exception");
        }

        private async Task<Snapshot> GetSnapshotIfExist(Guid aggregateId)
        {
            var sql = EventRepositoryScriptsAsStrings.GetAggregateSnapshot(aggregateId);
            var snapshot = _databaseProvider.QueryFirstOrDefaultAsync<Snapshot>(sql);
            return await snapshot;
        }

        private async Task<T> RecreateAggregateFromSnapshot<T>(Snapshot snapshot)
        {
            var sql = EventRepositoryScriptsAsStrings.GetEventsAfterSnapshot(snapshot);
            var listOfEventData = await _databaseProvider.QueryAsync<EventData>(sql, new { snapshot.AggregateId });
            var events = listOfEventData.OrderBy(e => e.Version).Select(x => x.DeserializeEvent());

            var aggregate = snapshot.DeserializeSnapshot<T>();

            return (T)_factory.CreateFromSnapshotAsync<T>(aggregate, events);
        }
    }
}
