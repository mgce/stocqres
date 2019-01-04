using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Events;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.Order
{
    public class SellOrder : Order
    {
        protected SellOrder(IEnumerable<IEvent> events) : base(events)
        {
        }

        public SellOrder(Guid walletId, Guid companyId, int quantity, string stockCode)
        {
            Publish(new SellOrderCreatedEvent(Guid.NewGuid(), walletId, companyId, quantity, stockCode));
        }

        private void Apply(SellOrderCreatedEvent @event)
        {
            Id = @event.AggregateId;
            WalletId = @event.WalletId;
            CompanyId = @event.CompanyId;
            Quantity = @event.Quantity;
            State = OrderState.Started;
            Type = OrderType.Buy;
        }

        private void Apply(SellOrderFinishedEvent @event)
        {
            State = OrderState.Finished;
        }

        public override void FinishOrder()
        {
            Publish(new SellOrderFinishedEvent(Id));
        }
    }
}
