using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.SharedKernel.Events
{
    public class StockToWalletAddedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public StockToWalletAddedEvent(Guid aggregateId, string name, string code, int unit, int quantity)
        {
            AggregateId = aggregateId;
            Name = name;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
