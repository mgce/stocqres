using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class BuyOrderCancelledEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string CancelReason { get; set; }

        public BuyOrderCancelledEvent(Guid aggregateId, string cancelReason)
        {
            AggregateId = aggregateId;
            CancelReason = cancelReason;
        }

        public BuyOrderCancelledEvent(string cancelReason)
        {
            CancelReason = cancelReason;
        }
    }
}
