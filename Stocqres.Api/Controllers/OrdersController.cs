using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.Transactions.Orders.Domain.Command;

namespace Stocqres.Api.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public OrdersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateOrderCommand command)
        {
            await _dispatcher.SendAsync(command);

            return Ok();
        }
    }
}
