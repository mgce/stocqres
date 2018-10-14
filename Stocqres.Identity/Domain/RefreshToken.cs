using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core;
using Stocqres.Core.Exceptions;

namespace Stocqres.Identity.Domain
{
    [Table("RefreshToken", Schema = "Identity")]
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; protected set; }
        public string Token { get; protected set; }
        public DateTime? RevokedAt { get; protected set; }
        public bool Revoked => RevokedAt.HasValue;

        protected RefreshToken()
        {
        }

        public RefreshToken(User user, IPasswordHasher<User> passwordHasher)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;
            CreatedAt = DateTime.UtcNow;
            Token = CreateToken(user, passwordHasher);
        }

        public void Revoke()
        {
            if (Revoked)
            {
                throw new StocqresException($"Refresh token: '{Id}' was already revoked at '{RevokedAt}'.");
            }
            RevokedAt = DateTime.UtcNow;
        }

        private static string CreateToken(User user, IPasswordHasher<User> passwordHasher)
            => passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N"))
                .Replace("=", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty);


    }
}
