using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
