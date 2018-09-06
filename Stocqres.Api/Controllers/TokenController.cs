using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Application.Token.Services;
using Stocqres.Core.Authentication;
using Stocqres.Core.Dispatcher;
using Stocqres.Domain.Commands.User;

namespace Stocqres.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SignIn command)
        {
            return Ok(await _tokenService.SignIn(command.Username, command.Password));
        }
    }
}
