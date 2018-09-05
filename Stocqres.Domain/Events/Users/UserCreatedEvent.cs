using System;
using Stocqres.Core.Events;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Events.Users
{
    public class UserCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
