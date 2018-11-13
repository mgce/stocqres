using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }

        public OrderCreatedEvent(Guid aggregateId, Guid walletId, Guid companyId, int quantity)
        {
            AggregateId = aggregateId;
            WalletId = walletId;
            CompanyId = companyId;
            Quantity = quantity;
        }
    }
}
