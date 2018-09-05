using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
