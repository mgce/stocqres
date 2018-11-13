using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain.Command
{
    public class FinishOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }

        public FinishOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
