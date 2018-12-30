using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Infrastructure.EventStore
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> GetFromAsync<T>(Guid aggregateId, int fromVersion = 0);
        Task SaveAsync<T>(IEnumerable<IEvent> events, Guid aggregateId, int actualVersion);
        Task ValidateVersionAsync<T>(Guid aggregateId, int originalVersion);
    }
}