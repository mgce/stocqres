using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.Customers.Wallet.Commands;
using CreateInvestorCommand = Stocqres.Identity.Domain.Commands.CreateInvestorCommand;

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

        [HttpPost("stocks")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(BuyStockCommand command)
        {
            await _dispatcher.SendAsync(command);

            return NoContent();
        }

        [HttpPost("wallet")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateWalletCommand command)
        {
            await _dispatcher.SendAsync(command);

            return NoContent();
        }
    }
}
