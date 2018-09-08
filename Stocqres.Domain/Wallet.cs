using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public virtual List<StockGroup> StockGroup { get; set; }
        public virtual User User { get; set; }

        protected Wallet()
        {}

        public Wallet(User user, decimal amount, Currency currency = Currency.PLN)
        {
            StockGroup = new List<StockGroup>();
            Amount = amount;
            Currency = currency;
            AssignUser(user);
        }

        public void AssignUser(User user)
        {
            if (user.Role != Role.Customer)
            {
                throw new StocqresException("User is not a customer");
            }
            User = user;
        }
    }
}
