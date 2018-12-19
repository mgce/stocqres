using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Companies.Domain.Commands;
using Stocqres.Customers.Companies.Presentation.Projections;
using Stocqres.Infrastructure.Projections;

namespace Stocqres.Api.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IProjectionReader _projectionReader;

        public CompanyController(IDispatcher dispatcher, IProjectionReader projectionReader)
        {
            _dispatcher = dispatcher;
            _projectionReader = projectionReader;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCompanyCommand command)
        {
            await _dispatcher.SendAsync(command);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{code}")]
        public async Task<ActionResult<CompanyProjection>> Get(string code)
        {
            return Ok(await _projectionReader.GetAsync<CompanyProjection>(p => p.StockCode == code));
        }
    }
}
