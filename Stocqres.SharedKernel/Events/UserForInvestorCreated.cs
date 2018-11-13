using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class UserForInvestorCreated : IEvent
    {
        public Guid AggregateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserForInvestorCreated(Guid userId, string firstName, string lastName)
        {
            AggregateId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
