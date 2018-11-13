using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain.Command
{
    public class CancelOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string CancelReason { get; set; }

        public CancelOrderCommand(Guid orderId, string cancelReason)
        {
            OrderId = orderId;
            CancelReason = cancelReason;
        }
    }
}
