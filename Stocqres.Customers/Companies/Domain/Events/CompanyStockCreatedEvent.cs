using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Companies.Domain.Events
{
    public class CompanyStockCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public CompanyStockCreatedEvent(Guid aggregateId, string code, int unit, int quantity)
        {
            AggregateId = aggregateId;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
