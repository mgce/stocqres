using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Investors.Events
{
    public class WalletToInvestorAssignedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid WalletId { get; set; }

        public WalletToInvestorAssignedEvent(Guid aggregateId, Guid walletId)
        {
            AggregateId = aggregateId;
            WalletId = walletId;
        }
    }
}
