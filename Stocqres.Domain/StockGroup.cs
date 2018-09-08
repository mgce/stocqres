using System;
using Stocqres.Core;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain
{
    public class StockGroup : BaseEntity
    {
        public Guid OwnerId { get; protected set; }
        public StockOwner StockOwner { get; protected set; }
        public int Quantity { get; protected set; }
        public Guid StockId { get; protected set; }

        protected StockGroup()
        {}

        public StockGroup(Guid ownerId, StockOwner stockOwner, int quantity, Guid stockId)
        {
            OwnerId = ownerId;
            StockOwner = stockOwner;
            Quantity = quantity;
            StockId = stockId;
        }

        public void UpdateQuantity(int quantity)
        {
            if(quantity < 0)
                throw new StocqresException("Quantity cannot be lower than zero");
            if (Quantity != quantity)
                Quantity = quantity;
        }

        public void DecreaseQuantity(int numberToDecrease)
        {
            if(Quantity - numberToDecrease < 0)
                throw new Exception("Quantity cannot be lower than zero");
            Quantity = Quantity - numberToDecrease;
        }
    }
}
