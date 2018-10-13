using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Customers.Investors.Presentation.Projections;
using Stocqres.Infrastructure.ProjectionWriter;

namespace Stocqres.Customers.Investors.Presentation.Handlers
{
    public class InvestorViewEventHandler : IEventHandler<InvestorCreatedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public InvestorViewEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }
        public async Task HandleAsync(InvestorCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new InvestorProjection(@event.AggregateId, @event.UserId, @event.FirstName,
                @event.LastName));
        }
    }
}
