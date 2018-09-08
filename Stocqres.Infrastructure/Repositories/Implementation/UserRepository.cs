using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
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
