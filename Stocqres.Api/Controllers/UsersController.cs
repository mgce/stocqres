using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stocqres.Core.Commands;
using Stocqres.Domain.Commands.User;

namespace Stocqres.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandBus _bus;

        public UsersController(ICommandBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            await _bus.Send(command);

            return NoContent();
        }
    }
}
