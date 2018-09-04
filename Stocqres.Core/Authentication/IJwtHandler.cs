using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Authentication
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null);
    }
}
