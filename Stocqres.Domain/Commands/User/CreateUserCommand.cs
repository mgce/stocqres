using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Domain.Commands.User
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
