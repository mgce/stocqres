using System;
using System.Linq;
using System.Threading.Tasks;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.EventSourcing;
using Stocqres.Core.Exceptions;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository.Scripts;
using Stocqres.Infrastructure.EventStore;
using Stocqres.Infrastructure.Snapshots;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly IAggregateRootFactory _factory;
        private readonly IEventBus _eventBus;
        private readonly IEventStore _eventStore;
        private readonly ISnapshotService _snapshotService;

        public EventRepository(IAggregateRootFactory factory, IEventBus eventBus, IEventStore eventStore, ISnapshotService snapshotService)
        {
            _factory = factory;
            _eventBus = eventBus;
            _eventStore = eventStore;
            _snapshotService = snapshotService;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            if(id == Guid.Empty)
                throw new StocqresException("Aggregate Id cannot be empty");

            var snapshot = await _snapshotService.GetLastAsync(id);
            if (snapshot != null)
                return await RecreateAggregateFromSnapshot<T>(snapshot);

            var events = await _eventStore.GetFromAsync<T>(id);

            if (events == null || !events.Any())
                throw new StocqresException("Aggregate doesn't exist");

            return (T)_factory.CreateAsync<T>(events);
        }

        public async Task SaveAsync<T>(T aggregate) where T : IAggregateRoot
        {
            var events = aggregate.GetUncommitedEvents();
            if (!events.Any())
                return;

            var originalVersion = aggregate.Version - events.Count + 1;

            await _eventStore.SaveAsync<T>(events, aggregate.Id, originalVersion);

            await RaiseEvents(aggregate);
        }

        public async Task TakeSnapshotAsync<T>(Guid aggregateId) where T : IAggregateRoot
        {
            var aggregate = await GetByIdAsync<T>(aggregateId);

            await _snapshotService.TakeAndSaveAsync(aggregate);
        }

        private async Task RaiseEvents(IAggregateRoot aggregate)
        {
            foreach (var @event in aggregate.GetUncommitedEvents())
            {
                await _eventBus.Publish(@event);
            }
        }

        private async Task<T> RecreateAggregateFromSnapshot<T>(Snapshot snapshot)
        {
            var events = await _eventStore.GetFromAsync<T>(snapshot.AggregateId, snapshot.SnapshottedVersion);

            var aggregate = snapshot.DeserializeSnapshot<T>();

            return (T)_factory.CreateFromSnapshotAsync<T>(aggregate, events);
        }
    }
}
