using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain.Command
{
    public class FinishSellOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }

        public FinishSellOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
