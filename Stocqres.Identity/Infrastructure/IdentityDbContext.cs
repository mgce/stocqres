using Microsoft.EntityFrameworkCore;
using Stocqres.Identity.Domain;

namespace Stocqres.Identity.Infrastructure
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
