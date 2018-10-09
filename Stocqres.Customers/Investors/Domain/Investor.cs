using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;

namespace Stocqres.Customers.Investors.Domain
{
    public class Investor : AggregateRoot
    {
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}