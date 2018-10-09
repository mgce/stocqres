using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Events.StockGroup
{
    public class StockGroupCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OwnerId { get; protected set; }
        public StockOwner StockOwner { get; protected set; }
        public int Quantity { get; protected set; }
        public Guid StockId { get; protected set; }
        public decimal Price { get; set; }

        public StockGroupCreatedEvent(Guid id, Guid ownerId, StockOwner stockOwner, int quantity, Guid stockId, decimal price)
        {
            AggregateId = id;
            OwnerId = ownerId;
            StockOwner = stockOwner;
            Quantity = quantity;
            StockId = stockId;
            Price = price;
        }

        
    }
}
