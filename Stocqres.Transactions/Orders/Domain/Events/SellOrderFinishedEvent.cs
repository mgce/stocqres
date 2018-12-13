using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class SellOrderFinishedEvent : IEvent
    {
        public Guid AggregateId { get; set; }

        public SellOrderFinishedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
