using System;
using System.Collections.Generic;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
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

        protected Order()
        {}

        protected Order(IEnumerable<IEvent> events) : base(events)
        {
        }

        public void CancelOrder(string cancelReason)
        {
            Publish(new BuyOrderCancelledEvent(Id, cancelReason));
        }

        public abstract void FinishOrder();

        private void Apply(BuyOrderCancelledEvent @event)
        {
            CancelReason = @event.CancelReason;
            State = OrderState.Cancelled;
        }

        private void Apply(BuyOrderFinishedEvent @event)
        {
            State = OrderState.Finished;
        }
    }
}
