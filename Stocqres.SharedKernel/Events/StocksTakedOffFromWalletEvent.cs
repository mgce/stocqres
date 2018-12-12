using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class StocksTakedOffFromWalletEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }

        public StocksTakedOffFromWalletEvent(Guid aggregateId, Guid companyId, int quantity)
        {
            AggregateId = aggregateId;
            CompanyId = companyId;
            Quantity = quantity;
        }
    }
}
