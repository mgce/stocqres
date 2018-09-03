using System;
using Stocqres.Core;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain
{
    public class StockGroup : BaseEntity
    {
        public Guid OwnerId { get; set; }
        public StockOwner StockOwner { get; set; }
        public int Quantity { get; set; }
        public Stock Stock { get; set; }

        protected StockGroup()
        {}

        public StockGroup(Guid ownerId, StockOwner stockOwner, int quantity, Stock stock)
        {
            OwnerId = ownerId;
            StockOwner = stockOwner;
            Quantity = quantity;
            Stock = stock;
        }
    }
}
