using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;   
using Microsoft.AspNetCore.Mvc;

namespace Stocqres.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Policy must be an Empty String in other case there will be throwed an Exception
    //that there is no handler
    //Of course we need also Authentication Schemes
    [Authorize(Policy = "", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        protected Guid InvestorId =>
            new Guid(User.Claims.FirstOrDefault(c => c.Type == "investorId")?.Value.ToUpperInvariant());

        protected Guid WalletId =>
            new Guid(User.Claims.FirstOrDefault(c => c.Type == "walletId")?.Value.ToUpperInvariant());
    }
}
