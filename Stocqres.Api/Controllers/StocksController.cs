using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Domain.Commands;
using Stocqres.Infrastructure;

namespace Stocqres.Api.Controllers
{
    public class StocksController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public StocksController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("/buy")]
        public async Task<IActionResult> Buy(BuyStockCommand command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
