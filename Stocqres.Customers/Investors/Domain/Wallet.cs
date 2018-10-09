using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;

namespace Stocqres.Customers.Investors.Domain
{
    public class Wallet : AggregateRoot
    {
        public Guid InvestorId { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public List<InvestorStock> StockList { get; set; }
    }
}
