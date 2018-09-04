using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Authentication
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}
