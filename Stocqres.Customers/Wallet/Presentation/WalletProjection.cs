using System;
using System.Collections.Generic;
using Stocqres.Core.Domain;
using Stocqres.Customers.Investors.Domain;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Wallet.Presentation
{
    public class WalletProjection : IProjection
    {
        public Guid Id { get; set; }
        public Guid InvestorId { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public List<Stock> StockList { get; set; }

        public WalletProjection(Guid id, Guid investorId, Currency currency, decimal amount)
        {
            Id = id;
            InvestorId = investorId;
            Currency = currency;
            Amount = amount;
        }
    }
}
