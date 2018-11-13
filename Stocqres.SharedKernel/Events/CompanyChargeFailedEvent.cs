using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class CompanyChargeFailedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public decimal AmountToRollback { get; set; }
        public string CancelReason { get; set; }

        public CompanyChargeFailedEvent(string cancelReason)
        {
            CancelReason = cancelReason;
        }
    }
}
