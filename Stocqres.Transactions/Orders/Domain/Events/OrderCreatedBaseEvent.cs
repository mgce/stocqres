using System;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class OrderCreatedBaseEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }

        public OrderCreatedBaseEvent(Guid aggregateId, Guid walletId, Guid companyId, int quantity)
        {
            AggregateId = aggregateId;
            WalletId = walletId;
            CompanyId = companyId;
            Quantity = quantity;
        }
    }
}
