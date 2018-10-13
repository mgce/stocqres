using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Investors.Domain.Commands
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
