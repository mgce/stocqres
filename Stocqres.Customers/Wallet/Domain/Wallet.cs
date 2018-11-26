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
        public Guid InvestorId { get; protected set; }
        public Currency Currency { get; protected set; }
        public decimal Amount { get; protected set; }
        public List<Stock> StockList { get; protected set; }

        public Wallet(Guid investorId, Currency currency, decimal amount)
        {
            Publish(new WalletCreatedEvent(Guid.NewGuid(), investorId, currency, amount));
        }

        protected Wallet(IEnumerable<IEvent> events) : base(events)
        {
        }

        public void ChargeWallet(Guid orderId, decimal amountToCharge)
        {
            if(amountToCharge <= 0)
                throw new StocqresException("Amount to charge cannot be lower or equal 0");

            if(amountToCharge > Amount)
                throw new StocqresException("You don't have enought money");

            Publish(new WalletChargedEvent(Id, orderId, amountToCharge));
        }

        public void AddStock(Guid orderId, string name, string code, int unit, int quantity)
        {
            if(string.IsNullOrEmpty(name))
                throw new StocqresException("Stock Name cannot be empty");
            if (string.IsNullOrEmpty(code))
                throw new StocqresException("Stock Code cannot be empty");
            if (unit <= 0)
                throw new StocqresException("Unit must be greater than zero");
            if (quantity <= 0)
                throw new StocqresException("Quantity must be greater than zero");

            Publish(new StockToWalletAddedEvent(Id, orderId, name, code, unit, quantity));
        }

        public void RollbackCharge(Guid orderId, decimal amount)
        {
            if(amount <= 0)
                throw new StocqresException("Amount to rollback must be greater than zero");

            Publish(new WalletChargeRollbackedEvent(Id, orderId, amount));
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
