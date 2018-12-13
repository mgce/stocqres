using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class BuyOrderFinishedEvent : IEvent
    {
        public Guid AggregateId { get; set; }

        public BuyOrderFinishedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
