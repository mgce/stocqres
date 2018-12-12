using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain.Command
{
    public class CreateBuyOrderCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }
    }
}
