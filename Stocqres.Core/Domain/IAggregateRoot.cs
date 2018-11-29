using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Core.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        int Version { get; }
        List<IEvent> GetUncommitedEvents();
        void ApplyEvents(IEnumerable<IEvent> events);
    }
}
