using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Identity.Repositories
{
    public interface IRefreshTokenRepository : IIdentityRepository<RefreshToken>
    {
    }
}
