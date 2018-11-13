using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain
{
    public class Order : AggregateRoot
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string CancelReason { get; set; }
        public OrderState State { get; set; }

        public Order(Guid walletId, Guid companyId, int quantity)
        {
            Publish(new OrderCreatedEvent(Guid.NewGuid(), walletId, companyId, quantity));
        }

        public void CancelOrder(string cancelReason)
        {
            Publish(new OrderCancelledEvent(Id, cancelReason));
        }

        public void FinishOrder()
        {
            Publish(new OrderFinishedEvent(Id));
        }

        private void Apply(OrderCreatedEvent @event)
        {
            Id = @event.AggregateId;
            WalletId = @event.WalletId;
            CompanyId = @event.CompanyId;
            Quantity = @event.Quantity;
            State = OrderState.Started;
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
