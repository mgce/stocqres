
using Stocqres.Core.Commands;

namespace Stocqres.Identity.Domain.Commands
{
    public class CreateInvestorCommand : ICommand
    {
        public string Username { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }   
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
