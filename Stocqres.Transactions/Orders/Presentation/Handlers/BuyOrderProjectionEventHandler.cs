using System;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.Projections;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Presentation.Handlers
{
    public class BuyOrderProjectionEventHandler : 
        IEventHandler<BuyOrderCreatedEvent>,
        IEventHandler<SellOrderCreatedEvent>,
        IEventHandler<BuyOrderCancelledEvent>,
        IEventHandler<BuyOrderFinishedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public BuyOrderProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }

        public async Task HandleAsync(BuyOrderCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new BuyOrderProjection(@event.AggregateId, @event.WalletId, @event.CompanyId, @event.Quantity,
                OrderState.Started));
        }

        public async Task HandleAsync(BuyOrderCancelledEvent @event)
        {
            await _projectionWriter.UpdateAsync<BuyOrderProjection>(@event.AggregateId,
                e => { e.State = OrderState.Cancelled; });
        }

        public async Task HandleAsync(BuyOrderFinishedEvent @event)
        {
            await _projectionWriter.UpdateAsync<BuyOrderProjection>(@event.AggregateId,
                e => { e.State = OrderState.Finished; });
        }

        public async Task HandleAsync(SellOrderCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new BuyOrderProjection(@event.AggregateId, @event.WalletId, @event.CompanyId, @event.Quantity,
                OrderState.Started));
        }
    }
}
