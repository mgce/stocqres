using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class OrderFinishedEvent : IEvent
    {
        public Guid AggregateId { get; set; }

        public OrderFinishedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
