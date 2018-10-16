using System;
using System.Collections.Generic;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Wallet.Events;
using Stocqres.SharedKernel.Events;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Wallet.Domain
{
    public class Wallet : AggregateRoot
    {
        public Guid InvestorId { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public List<Stock> StockList { get; set; }

        public Wallet(Guid investorId, Currency currency, decimal amount)
        {
            Publish(new WalletCreatedEvent(Guid.NewGuid(), investorId, currency, amount));
        }

        public void ChargeWallet(decimal amountToCharge)
        {
            if(amountToCharge > Amount)
                throw new StocqresException("You don't have enought money");

            Publish(new WalletChargedEvent(Id, amountToCharge));
        }

        private void Apply(WalletChargedEvent @event)
        {
            Amount -= @event.Amount;
        }

        private void Apply(WalletCreatedEvent @event)
        {
            InvestorId = @event.InvestorId;
            Currency = @event.Currency;
            Amount = @event.Amount;
        }
    }
}
