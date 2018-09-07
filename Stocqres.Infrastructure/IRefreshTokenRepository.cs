using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
    }
}
