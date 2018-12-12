using System;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.Order
{
    public abstract class Order : AggregateRoot
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }
        public string CancelReason { get; set; }
        public string StockCode { get; set; }
        public OrderState State { get; set; }
        public OrderType Type { get; set; }

        public void CancelOrder(string cancelReason)
        {
            Publish(new OrderCancelledEvent(Id, cancelReason));
        }

        public void FinishOrder()
        {
            Publish(new OrderFinishedEvent(Id));
        }

        private void Apply(OrderCancelledEvent @event)
        {
            CancelReason = @event.CancelReason;
            State = OrderState.Cancelled;
        }

        private void Apply(OrderFinishedEvent @event)
        {
            State = OrderState.Finished;
        }
    }
}
