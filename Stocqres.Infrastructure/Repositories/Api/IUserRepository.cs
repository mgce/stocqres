using System;
using System.Threading.Tasks;
using Stocqres.Domain;

namespace Stocqres.Infrastructure.Repositories.Api
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync(Guid userId);
    }
}
