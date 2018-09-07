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
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenController(ITokenService tokenService, 
            IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(SignIn command)
        {
            return Ok(await _tokenService.SignInAsync(command.Username, command.Password));
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command)
            => Ok(await _tokenService.SignInAsync(command.Username, command.Password));

        [HttpPost("refresh-tokens/{refreshToken}/refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
            => Ok(await _refreshTokenService.CreateAccessTokenAsync(refreshToken));
    }
}
