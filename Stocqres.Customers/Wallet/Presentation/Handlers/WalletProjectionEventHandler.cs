using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Wallet.Events;
using Stocqres.Infrastructure.ProjectionWriter;

namespace Stocqres.Customers.Wallet.Presentation.Handlers
{
    public class WalletProjectionEventHandler : IEventHandler<WalletCreatedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public WalletProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }
        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            var projection = new WalletProjection(
                @event.AggregateId, @event.InvestorId, @event.Currency, @event.Amount);
            await _projectionWriter.AddAsync(projection);
        }
    }
}
