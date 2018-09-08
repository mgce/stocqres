using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Domain.Commands.User;
using Stocqres.Domain.Commands.Wallet;

namespace Stocqres.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public UsersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            await _dispatcher.SendAsync(command);

            return NoContent();
        }

        [HttpPost("/wallet")]
        public async Task<IActionResult> CreateWallet(CreateWalletCommand command)
        {
            await _dispatcher.SendAsync(command);

            return NoContent();
        }
    }
}
