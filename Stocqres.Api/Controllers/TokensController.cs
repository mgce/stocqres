using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Authentication;
using Stocqres.Core.Dispatcher;
using Stocqres.Identity.Application.Services;

namespace Stocqres.Api.Controllers
{
    public class TokensController : BaseController
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthenticationService _authenticationService;

        public TokensController(IRefreshTokenService refreshTokenService, IAuthenticationService authenticationService)
        {
            _refreshTokenService = refreshTokenService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(SignIn command)
        {
            return Ok(await _authenticationService.SignInAsync(command.Username, command.Password));
        }

        [AllowAnonymous]
        [HttpGet("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command)
            => Ok(await _authenticationService.SignInAsync(command.Username, command.Password));

        [AllowAnonymous]
        [HttpGet("refresh-tokens/{refreshToken}/refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
            => Ok(await _refreshTokenService.CreateAccessTokenAsync(refreshToken));
    }
}
