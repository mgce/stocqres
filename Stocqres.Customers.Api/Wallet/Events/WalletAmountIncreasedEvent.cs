using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Wallet.Events
{
    public class WalletAmountIncreasedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public decimal Amount { get; set; }

        public WalletAmountIncreasedEvent(Guid id, decimal amount)
        {
            AggregateId = id;
            Amount = amount;
        }
    }
}
