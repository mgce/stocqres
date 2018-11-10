using System;
using System.Collections.Generic;
using System.Linq;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
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

        protected Wallet(IEnumerable<IEvent> events) : base(events)
        {
        }

        public void ChargeWallet(decimal amountToCharge)
        {
            if(amountToCharge > Amount)
                throw new StocqresException("You don't have enought money");

            Publish(new WalletChargedEvent(Id, amountToCharge));
        }

        public void AddStock(string name, string code, int unit, int quantity)
        {
            if(string.IsNullOrEmpty(name))
                throw new StocqresException("Stock Name cannot be empty");
            if (string.IsNullOrEmpty(code))
                throw new StocqresException("Stock Code cannot be empty");
            if (unit <= 0)
                throw new StocqresException("Unit must be greater than zero");
            if (quantity > Amount)
                throw new StocqresException("Quantity must be greater than zero");

            Publish(new StockToWalletAddedEvent(Id, name, code, unit, quantity));
        }

        public void RollbackCharge(decimal amount)
        {
            if(amount <= 0)
                throw new StocqresException("Amount to rollback must be greater than zero");

            Publish(new WalletChargeRollbackedEvent(Id, amount));
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
            StockList = new List<Stock>();
        }

        private void Apply(StockToWalletAddedEvent @event)
        {
            var existingStock = StockList.SingleOrDefault(s => s.Code == @event.Code);
            if (existingStock != null)
                existingStock.Quantity += @event.Quantity;
            else
            {
                StockList.Add(new Stock(@event.Name, @event.Code, @event.Unit, @event.Quantity));
            }
        }

        private void Apply(WalletChargeRollbackedEvent @event)
        {
            Amount += @event.Amount;
        }
    }
}
