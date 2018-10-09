using Stocqres.Identity.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Identity.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
    }
}
