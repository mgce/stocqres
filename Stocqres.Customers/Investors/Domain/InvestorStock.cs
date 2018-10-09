using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core;

namespace Stocqres.Customers.Investors.Domain
{
    public class InvestorStock : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
