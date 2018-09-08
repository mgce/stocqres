using System;
using Stocqres.Core.Events;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Events.Wallet
{
    public class WalletCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public WalletCreatedEvent(Guid walletId, Guid userId, decimal amount, Currency currency)
        {
            Id = walletId;
            UserId = userId;
            Amount = amount;
            Currency = currency;
        }

        
    }
}
