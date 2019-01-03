using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Investors.Events
{
    public class InvestorCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public InvestorCreatedEvent(Guid aggregateId, Guid userId, string firstName, string lastName)
        {
            AggregateId = aggregateId;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
