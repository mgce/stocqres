﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Wallet.Commands;
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

        [HttpPost("wallet")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateWalletCommand command)
        {
            await _dispatcher.SendAsync(command);

            return Ok();
        }

        [HttpGet("/{id}/wallet")]
        public async Task<IEnumerable<WalletProjection>> Get(Guid id)
        {
            return await _projectionReader.FindAsync<WalletProjection>(w => w.InvestorId == id);
        }
    }
}
