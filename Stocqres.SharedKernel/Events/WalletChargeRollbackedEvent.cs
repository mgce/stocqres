using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class WalletChargeRollbackedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public decimal Amount { get; set; }

        public WalletChargeRollbackedEvent(Guid aggregateId, decimal amount)
        {
            AggregateId = aggregateId;
            Amount = amount;
        }
    }
}
