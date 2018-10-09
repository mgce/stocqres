using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Identity.Domain;
using Stocqres.Infrastructure.Repositories.Api;
using Stocqres.Infrastructure.Repositories.Implementation;

namespace Stocqres.Identity.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await GetAsync(u => u.Id == userId);
        }
    }
}
