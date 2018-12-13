using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain.Command
{
    public class FinishBuyOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }

        public FinishBuyOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
