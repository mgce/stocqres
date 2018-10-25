﻿using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Investors.Domain
{
    public class Investor : AggregateRoot
    {
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Stock> Stocks { get; set; }

        public Investor(Guid userId, string firstName, string lastName)
        {
            Publish(new InvestorCreatedEvent(Guid.NewGuid(), userId, firstName, lastName));
        }

        protected Investor(IEnumerable<IEvent> events) : base(events)
        {
        }

        public void AssignWallet(Guid walletId)
        {
            Publish(new WalletToInvestorAssignedEvent(Id, walletId));
        }

        private void Apply(InvestorCreatedEvent @event)
        {
            Id = @event.AggregateId;
            UserId = @event.UserId;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
        }

        private void Apply(WalletToInvestorAssignedEvent @event)
        {
            WalletId = @event.WalletId;
        }


    }
}