using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Domain;
using Stocqres.Domain.Enums;
using Stocqres.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Api.Controllers
{
    public class StockGroupsController : BaseController
    {
        private readonly IStockGroupRepository _stockGroupRepository;

        public StockGroupsController(IStockGroupRepository stockGroupRepository)
        {
            _stockGroupRepository = stockGroupRepository;
        }

        [HttpGet("{stockGroupId}")]
        public async Task<IActionResult> Get(Guid stockGroupId)
        {
            var stockGroup = await _stockGroupRepository
                .GetAsync(sg=>sg.StockOwner == StockOwner.StockExchange || sg.OwnerId == UserId);
            return Ok(stockGroup);
        }
    }
}
