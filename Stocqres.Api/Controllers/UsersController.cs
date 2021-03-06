﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Identity.Domain.Commands;

namespace Stocqres.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public UsersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        //[HttpPost("")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Create(CreateUserCommand command)
        //{
        //    await _dispatcher.SendAsync(command);

        //    return Ok();
        //}

        //[HttpPost("investor")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Create(CreateInvestorCommand command)
        //{
        //    await _dispatcher.SendAsync(command);

        //    return Ok();
        //}
    }
}
