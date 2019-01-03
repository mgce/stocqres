using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Investors.Commands
{
    public class CreateInvestorCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CreateInvestorCommand(Guid userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
