using System;
using Stocqres.Core.Events;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Events.Wallet
{
    public class WalletCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Domain.Wallet Wallet { get; set; }

        public WalletCreatedEvent(Domain.Wallet wallet)
        {
            Wallet = wallet;
        }
    }
}
