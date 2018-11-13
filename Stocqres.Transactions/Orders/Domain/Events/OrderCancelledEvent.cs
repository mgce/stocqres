using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class OrderCancelledEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string CancelReason { get; set; }

        public OrderCancelledEvent(Guid aggregateId, string cancelReason)
        {
            AggregateId = aggregateId;
            CancelReason = cancelReason;
        }

        public OrderCancelledEvent(string cancelReason)
        {
            CancelReason = cancelReason;
        }
    }
}
