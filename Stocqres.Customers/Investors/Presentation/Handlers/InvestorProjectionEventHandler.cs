using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Api.Investors.Events;
using Stocqres.Customers.Api.Investors.Presentation;
using Stocqres.Customers.Api.Wallet.Events;
using Stocqres.Infrastructure.Projections;

namespace Stocqres.Customers.Investors.Presentation.Handlers
{
    public class InvestorProjectionEventHandler : IEventHandler<InvestorCreatedEvent>, IEventHandler<WalletCreatedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public InvestorProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }
        public async Task HandleAsync(InvestorCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new InvestorProjection(@event.AggregateId, @event.UserId, @event.FirstName,
                @event.LastName));
        }

        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            await _projectionWriter.UpdateAsync<InvestorProjection>(@event.InvestorId,
                e => e.WalletId = @event.AggregateId);
        }
    }
}
