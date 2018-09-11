using System;
using Stocqres.Core.Commands;

namespace Stocqres.Domain.Commands
{
    public class SellStocksCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
