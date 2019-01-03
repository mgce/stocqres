using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Companies.Events
{
    public class CompanyCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string Name { get; set; }

        public CompanyCreatedEvent(Guid aggregateId, string name)
        {
            AggregateId = aggregateId;
            Name = name;
        }
    }
}
