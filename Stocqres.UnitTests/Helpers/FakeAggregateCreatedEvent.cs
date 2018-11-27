using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.UnitTests.Helpers
{
    public class FakeAggregateCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }

        public FakeAggregateCreatedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
