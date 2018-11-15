using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class CompanyChargeFailedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid OrderId { get; set; }
        public string CancelReason { get; set; }

        public CompanyChargeFailedEvent(Guid orderId, string cancelReason)
        {
            OrderId = orderId;
            CancelReason = cancelReason;
        }
    }
}
