using System;
using System.Threading.Tasks;
using Marten;
using MongoDB.Driver;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;
using Stocqres.Infrastructure.Repositories.Implementation;

namespace Stocqres.Identity.Repositories
{
    public class UserRepository : IdentityRepository<User>, IUserRepository
    {
        public UserRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await FindByIdAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Set<User>().SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
