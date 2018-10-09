using System;
using Stocqres.Core.Events;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Events.Users
{
    public class UserCreatedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public UserCreatedEvent(Guid id, string username, string email, Role role)
        {
            AggregateId = id;
            Username = username;
            Email = email;
            Role = role;
        }
    }
}
