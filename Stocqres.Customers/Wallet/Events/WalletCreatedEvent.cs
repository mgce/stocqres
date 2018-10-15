using System;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain;

namespace Stocqres.Customers.Wallet.Events
{
    public class WalletCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid InvestorId { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }

        public WalletCreatedEvent(Guid aggregateId, Guid investorId, Currency currency, decimal amount)
        {
            AggregateId = aggregateId;
            InvestorId = investorId;
            Currency = currency;
            Amount = amount;
        }
    }
}
