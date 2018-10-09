using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Authentication
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, IDictionary<string, string> claims = null);
    }
}
