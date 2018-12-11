using System;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.Order
{
    public class Order : AggregateRoot
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string CancelReason { get; set; }
        public OrderState State { get; set; }
        public OrderType Type { get; set; }

        public Order(Guid walletId, Guid companyId, int quantity, OrderType type)
        {
            switch (type)
            {
                case OrderType.Buy:
                    Publish(new BuyOrderCreatedEvent(Guid.NewGuid(), walletId, companyId, quantity));
                    break;
                case OrderType.Sell:
                    Publish(new BuyOrderCreatedEvent(Guid.NewGuid(), walletId, companyId, quantity));
                    break;
                default:
                    throw new StocqresException("Invalid order type");
            }
            
        }

        public void CancelOrder(string cancelReason)
        {
            Publish(new OrderCancelledEvent(Id, cancelReason));
        }

        public void FinishOrder()
        {
            Publish(new OrderFinishedEvent(Id));
        }

        private void Apply(BuyOrderCreatedEvent @event)
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
