using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class SellOrderCreatedEvent : OrderCreatedBaseEvent
    {
        public string StockCode { get; set; }

        public SellOrderCreatedEvent(Guid aggregateId, Guid walletId, Guid companyId, int quantity, string stockCode) 
            : base(aggregateId, walletId, companyId, quantity)
        {
            StockCode = stockCode;
        }
    }
}
