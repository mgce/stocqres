using MongoDB.Driver;
using Stocqres.Identity.Domain;
using Stocqres.Infrastructure.Repositories.Api;
using Stocqres.Infrastructure.Repositories.Implementation;

namespace Stocqres.Identity.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
