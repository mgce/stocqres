using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Domain.Events.StockGroup
{
    public class StockGroupQuantityChangedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public int Quantity { get; set; }

        public StockGroupQuantityChangedEvent(Guid id, int quantity)
        {
            AggregateId = id;
            Quantity = quantity;
        }
    }
}
