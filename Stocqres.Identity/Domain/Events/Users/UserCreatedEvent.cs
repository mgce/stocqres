using System;
using Stocqres.Core.Events;

namespace Stocqres.Identity.Domain.Events.Users
{
    public class UserCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(Guid id, string username, string email)
        {
            AggregateId = id;
            Username = username;
            Email = email;
        }
    }
}
