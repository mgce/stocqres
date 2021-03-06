﻿using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Domain.Events
{
    public class BuyOrderCreatedEvent : OrderCreatedBaseEvent
    {
        public BuyOrderCreatedEvent(Guid aggregateId, Guid walletId, Guid companyId, int quantity) 
            : base(aggregateId, walletId, companyId, quantity)
        {
        }
    }
}
