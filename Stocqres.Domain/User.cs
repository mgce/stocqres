using System;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain
{
    public class User : AggregateRoot
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public Guid WalletId { get; set; }

        public User()
        {}

        public User(string username, string email, Role role)
        {
            Username = username;
            Email = email.ToLowerInvariant();
            Role = role;
        }

        public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new StocqresException("Password cannot be empty");
            }

            Password = passwordHasher.HashPassword(this, password);
        }

        public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        {
            return passwordHasher.VerifyHashedPassword(this, Password, password) != PasswordVerificationResult.Failed;
        }
    }
}
