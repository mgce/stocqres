using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Customers.Investors.Presentation.Projections;
using Stocqres.Customers.Wallet.Events;
using Stocqres.Infrastructure.ProjectionWriter;

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
