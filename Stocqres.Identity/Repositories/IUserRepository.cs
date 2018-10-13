using System;
using System.Threading.Tasks;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Identity.Repositories
{
    public interface IUserRepository : IIdentityRepository<User>
    {
        Task<User> GetUserAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
    }
}
