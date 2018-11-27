using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.UnitTests.Helpers
{
    public class FakeMethodAppliedEvent : IEvent
    {
        public Guid AggregateId { get; set; }

        public FakeMethodAppliedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
