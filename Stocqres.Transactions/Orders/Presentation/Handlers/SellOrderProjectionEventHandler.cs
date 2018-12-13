using System;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.Projections;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Presentation.Handlers
{
    public class SellOrderProjectionEventHandler : 
        IEventHandler<SellOrderCreatedEvent>,
        IEventHandler<SellOrderFinishedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public SellOrderProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }

        public async Task HandleAsync(SellOrderCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new SellOrderProjection(@event.AggregateId, @event.WalletId, @event.CompanyId, @event.Quantity,
                OrderState.Started));
        }

        public async Task HandleAsync(SellOrderFinishedEvent @event)
        {
            await _projectionWriter.UpdateAsync<SellOrderProjection>(@event.AggregateId,
                e => { e.State = OrderState.Finished; });
        }
    }
}
