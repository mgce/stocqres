using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
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
