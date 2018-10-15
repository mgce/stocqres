using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Wallet.Events
{
    public class WalletAmountDecreasedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public decimal Amount { get; set; }

        public WalletAmountDecreasedEvent(Guid id, decimal amount)
        {
            AggregateId = id;
            Amount = amount;
        }
    }
}
