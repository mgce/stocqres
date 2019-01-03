using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Wallet.Events
{
    public class StockToWalletAddedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OrderId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public StockToWalletAddedEvent(Guid aggregateId, Guid orderId, Guid companyId, string name, string code, int unit, int quantity)
        {
            AggregateId = aggregateId;
            OrderId = orderId;
            CompanyId = companyId;
            Name = name;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
