using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class SellOrderCancelledEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string CancelReason { get; set; }

        public SellOrderCancelledEvent(Guid aggregateId, string cancelReason)
        {
            AggregateId = aggregateId;
            CancelReason = cancelReason;
        }

        public SellOrderCancelledEvent(string cancelReason)
        {
            CancelReason = cancelReason;
        }
    }
}
