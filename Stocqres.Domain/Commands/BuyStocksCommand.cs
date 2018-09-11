using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Domain.Commands
{
    public class BuyStocksCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
