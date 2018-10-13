using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Identity.Domain.Commands;

namespace Stocqres.Api.Controllers
{
    public class InvestorsController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public InvestorsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateInvestorCommand command)
        {
            await _dispatcher.SendAsync(command);

            return NoContent();
        }
    }
}
