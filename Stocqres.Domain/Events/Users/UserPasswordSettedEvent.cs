﻿using System;

namespace Stocqres.Identity.Domain.Events.Users
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
