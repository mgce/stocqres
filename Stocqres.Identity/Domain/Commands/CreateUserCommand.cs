using Stocqres.Core.Commands;

namespace Stocqres.Identity.Domain.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
