using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Domain.Events.Wallet
{
    public class WalletAmountIncreasedEvent : IEvent
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }

        public WalletAmountIncreasedEvent(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
