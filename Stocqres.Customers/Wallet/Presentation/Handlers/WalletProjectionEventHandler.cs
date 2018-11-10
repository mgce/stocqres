using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Wallet.Events;
using Stocqres.Infrastructure.ProjectionWriter;
using Stocqres.SharedKernel.Events;

namespace Stocqres.Customers.Wallet.Presentation.Handlers
{
    public class WalletProjectionEventHandler : IEventHandler<WalletCreatedEvent>, IEventHandler<WalletChargeRollbackedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public WalletProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }
        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new WalletProjection(
                @event.AggregateId, @event.InvestorId, @event.Currency, @event.Amount));
        }

        public async Task HandleAsync(WalletChargeRollbackedEvent @event)
        {
            await _projectionWriter.UpdateAsync<WalletProjection>(@event.AggregateId, e => e.Amount += @event.Amount);
        }
    }
}
