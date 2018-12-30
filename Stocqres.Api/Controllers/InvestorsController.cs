using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Api.Investors.Presentation;
using Stocqres.Customers.Api.Wallet.Commands;
using Stocqres.Customers.Api.Wallet.Presentation;
using Stocqres.Customers.Wallet.Presentation;
using Stocqres.Identity.Domain.Commands;
using Stocqres.Infrastructure.Projections;

namespace Stocqres.Api.Controllers
{
    public class InvestorsController : BaseController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IProjectionReader _projectionReader;

        public InvestorsController(IDispatcher dispatcher, IProjectionReader projectionReader)
        {
            _dispatcher = dispatcher;
            _projectionReader = projectionReader;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateInvestorCommand command)
        {
            await _dispatcher.SendAsync(command);

            return Ok();
        }

        [HttpGet("")]
        public async Task<InvestorProjection> GetInvestor()
        {
            return await _projectionReader.GetAsync<InvestorProjection>(w => w.Id == InvestorId);
        }

        [HttpPost("wallet")]
        public async Task<IActionResult> Post(CreateWalletCommand command)
        {
            command.InvestorId = InvestorId;
            await _dispatcher.SendAsync(command);

            return Ok();
        }

        [HttpGet("wallet")]
        public async Task<WalletProjection> GetWallet()
        {
            return await _projectionReader.GetAsync<WalletProjection>(w => w.InvestorId == InvestorId);
        }
    }
}
