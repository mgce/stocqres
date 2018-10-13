using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;
using Stocqres.Customers.Investors.Domain.Events;

namespace Stocqres.Customers.Investors.Domain
{
    public class Investor : AggregateRoot
    {
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Investor(Guid userId,  string firstName, string lastName)
        {
            Publish(new InvestorCreatedEvent(Guid.NewGuid(), UserId, FirstName, LastName));
        }

        private void ApplyEvent(InvestorCreatedEvent @event)
        {
            Id = @event.AggregateId;
            UserId = @event.UserId;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
        }
    }
}