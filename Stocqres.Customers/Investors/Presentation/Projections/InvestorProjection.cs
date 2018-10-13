using System;

namespace Stocqres.Customers.Investors.Presentation.Projections
{
    public class InvestorProjection
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
