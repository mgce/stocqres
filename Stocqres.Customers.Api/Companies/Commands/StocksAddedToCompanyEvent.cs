using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Companies.Commands
{
    public class StocksAddedToCompanyEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OrderId { get; set; }
        public int StockQuantity { get; set; }

        public StocksAddedToCompanyEvent(Guid aggregateId, Guid orderId, int stockQuantity)
        {
            AggregateId = aggregateId;
            OrderId = orderId;
            StockQuantity = stockQuantity;
        }
    }
}
