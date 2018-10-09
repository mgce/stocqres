using System;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;
using Stocqres.Domain.Events.Users;
using Stocqres.Domain.Events.Wallet;

namespace Stocqres.Domain
{
    public class User : AggregateRoot
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Wallet Wallet { get; set; }

        public User()
        {}

        public User(string username, string email, Role role)
        {
            if(string.IsNullOrEmpty(username))
                throw new StocqresException("Username cannot be empty");

            if (string.IsNullOrEmpty(email))
                throw new StocqresException("Email cannot be empty");

            Publish(new UserCreatedEvent(Guid.NewGuid(), username, email.ToLowerInvariant(), role));
        }

        public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new StocqresException("Password cannot be empty");
            }
            
            var hashedPassword = passwordHasher.HashPassword(this, password);

            Publish(new UserPasswordSettedEvent(hashedPassword));
        }

        public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        {
            return passwordHasher.VerifyHashedPassword(this, Password, password) != PasswordVerificationResult.Failed;
        }

        public void AssignWallet(Wallet wallet)
        {
            Publish(new WalletCreatedEvent(wallet));
        }

        private void ApplyEvent(UserCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Username = @event.Username;
            Email = @event.Email;
            Role = @event.Role;
        }

        private void ApplyEvent(UserPasswordSettedEvent @event)
        {
            Password = @event.Password;
        }

        private void ApplyEvent(WalletCreatedEvent @event)
        {
            Wallet = @event.Wallet;
        }
    }
}
