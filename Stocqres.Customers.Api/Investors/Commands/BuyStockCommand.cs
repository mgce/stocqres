using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Investors.Commands
{
    public class BuyStockCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
