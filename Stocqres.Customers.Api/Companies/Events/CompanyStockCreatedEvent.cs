using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Companies.Events
{
    public class CompanyStockCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid StockId { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public CompanyStockCreatedEvent(Guid aggregateId, Guid stockId,string code, int unit, int quantity)
        {
            StockId = stockId;
            AggregateId = aggregateId;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
