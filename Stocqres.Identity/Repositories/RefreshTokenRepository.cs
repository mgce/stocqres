using MongoDB.Driver;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;
using Stocqres.Infrastructure.Repositories.Implementation;

namespace Stocqres.Identity.Repositories
{
    public class RefreshTokenRepository : IdentityRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }
    }
}
