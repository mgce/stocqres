using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.Order
{
    public class BuyOrder : Order
    {
        protected BuyOrder(IEnumerable<IEvent> events) : base(events)
        {
        }

        public BuyOrder(Guid walletId, Guid companyId, int quantity)
        {
            Publish(new BuyOrderCreatedEvent(Guid.NewGuid(), walletId, companyId, quantity));
        }

        private void Apply(BuyOrderCreatedEvent @event)
        {
            Id = @event.AggregateId;
            WalletId = @event.WalletId;
            CompanyId = @event.CompanyId;
            Quantity = @event.Quantity;
            State = OrderState.Started;
            Type = OrderType.Buy;
        }

        private void Apply(BuyOrderFinishedEvent @event)
        {
            State = OrderState.Finished;
        }

        public override void FinishOrder()
        {
            Publish(new BuyOrderFinishedEvent(Id));
        }
    }
}
