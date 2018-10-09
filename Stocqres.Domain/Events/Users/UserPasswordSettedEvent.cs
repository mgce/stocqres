using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;

namespace Stocqres.Domain.Events.Users
{
    public class UserPasswordSettedEvent : IEvent
    {
        public string Password { get; set; }

        public UserPasswordSettedEvent(string password)
        {
            Password = password;
        }

        public Guid AggregateId { get; set; }
    }
}
