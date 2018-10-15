using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Investors.Domain.Commands
{
    public class BuyStockCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
