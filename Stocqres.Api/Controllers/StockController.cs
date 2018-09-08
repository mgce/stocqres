using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Domain.Commands;

namespace Stocqres.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public StockController(IDispatcher dispatcher)
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
