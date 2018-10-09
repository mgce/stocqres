using System;
using System.Threading.Tasks;
using Stocqres.Identity.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Identity.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync(Guid userId);
    }
}
