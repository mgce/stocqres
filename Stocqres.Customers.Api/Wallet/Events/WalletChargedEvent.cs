using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Wallet.Events
{
    public class WalletChargedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }

        public WalletChargedEvent(Guid aggregateId, Guid orderId, decimal amount)
        {
            AggregateId = aggregateId;
            OrderId = orderId;
            Amount = amount;
        }      
    }
}
