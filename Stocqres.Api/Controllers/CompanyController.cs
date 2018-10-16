using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Companies.Domain.Commands;

namespace Stocqres.Api.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public CompanyController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCompanyCommand command)
        {
            await _dispatcher.SendAsync(command);
            return Ok();
        }
    }
}
