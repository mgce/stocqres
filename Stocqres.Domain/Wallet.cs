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
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public virtual List<StockGroup> StockGroup { get; set; }
        public virtual User User { get; set; }

        protected Wallet()
        {}

        public Wallet( decimal amount, Currency currency = Currency.PLN)
        {
            StockGroup = new List<StockGroup>();
            Amount = amount;
            Currency = currency;
        }

        public bool HaveEnoughtMoney(decimal price, int unit, int quantity)
        {
            return price * unit * quantity> Amount;
        }

        public void DecreaseAmount(decimal value)
        {
            if (Amount < value)
                throw new Exception("Your wallet amount is too low");
            Amount -= value;
        }

        public void IncreaseAmount(decimal value)
        {
            Amount += value;
        }

        
    }
}
