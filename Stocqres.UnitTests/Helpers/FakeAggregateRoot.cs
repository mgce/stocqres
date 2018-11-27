using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;

namespace Stocqres.UnitTests.Helpers
{
    public class FakeAggregateRoot : AggregateRoot
    {
        public FakeAggregateRoot(Guid aggregateId)
        {
            Publish(new FakeAggregateCreatedEvent(aggregateId));
        }

        public void FakeMethod()
        {
            Publish(new FakeMethodAppliedEvent(Id));
        }

        private void Apply(FakeAggregateCreatedEvent @event)
        {
            Id = @event.AggregateId;
        }

        private void Apply(FakeMethodAppliedEvent @event)
        {
        }
    }
}
