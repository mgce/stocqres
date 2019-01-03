using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Wallet.Events
{
    public class WalletChargeRollbackedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }

        public WalletChargeRollbackedEvent(Guid aggregateId, Guid orderId, decimal amount)
        {
            AggregateId = aggregateId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}
