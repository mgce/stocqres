using System;
using Stocqres.Core.Domain;

namespace Stocqres.Customers.Api.Investors.Presentation
{
    public class InvestorProjection : IProjection
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public InvestorProjection(Guid id, Guid userId, string firstName, string lastName)
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
