using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.EventRepository.Scripts;

namespace Stocqres.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IDatabaseProvider _databaseProvider;

        public EventStore(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }
        public async Task<IEnumerable<IEvent>> GetFromAsync<T>(Guid aggregateId, int fromVersion = 0)
        {
            var getAggregateSql = EventRepositoryScriptsAsStrings.GetFromVersion(typeof(T).Name, aggregateId, fromVersion);
            var listOfEventData = await _databaseProvider.QueryAsync<EventData>(getAggregateSql, new { aggregateId });

            if (listOfEventData == null || !listOfEventData.Any())
                throw new StocqresException("Aggregate doesn't exist");

            return listOfEventData.OrderBy(e => e.Version).Select(x => x.DeserializeEvent());
        }

        public async Task SaveAsync<T>(IEnumerable<IEvent> events, Guid aggregateId, int originalVersion)
        {
            var validator = ValidateVersionAsync<T>(aggregateId, originalVersion);

            var aggregateType = typeof(T).Name;

            var eventsToSave = events.Select(e => e.ToEventData(aggregateId, aggregateType, originalVersion++, DateTime.Now));

            string insertScript = EventRepositoryScriptsAsStrings.InsertIntoAggregate(aggregateType);

            await validator;

            await _databaseProvider.ExecuteAsync(insertScript, eventsToSave);
        }

        public async Task ValidateVersionAsync<T>(Guid aggregateId, int originalVersion)
        {
            var aggregateTypeName = typeof(T).Name;

            var foundVersionQuery =
                EventRepositoryScriptsAsStrings.FindAggregateVersion(aggregateTypeName, aggregateId);
            var foundVersionResult = await _databaseProvider.ExecuteScalarAsync(foundVersionQuery);
            if ((int?)foundVersionResult >= originalVersion)
                throw new Exception("Concurrency Exception");
        }
    }
}
