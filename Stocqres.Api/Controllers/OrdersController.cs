using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Dispatcher;
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

        [HttpPost("buy")]
        public async Task<IActionResult> Post(CreateBuyOrderCommand command)
        {
            command.WalletId = WalletId;
            await _dispatcher.SendAsync(command);

            return Ok();
        }


        [HttpPost("sell")]
        public async Task<IActionResult> Post(CreateSellOrderCommand command)
        {
            command.WalletId = WalletId;
            await _dispatcher.SendAsync(command);

            return Ok();
        }
    }
}
