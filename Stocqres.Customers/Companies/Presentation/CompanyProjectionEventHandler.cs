using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Companies.Domain.Events;
using Stocqres.Customers.Companies.Presentation.Projections;
using Stocqres.Infrastructure.ProjectionWriter;

namespace Stocqres.Customers.Companies.Presentation
{
    public class CompanyProjectionEventHandler : IEventHandler<CompanyCreatedEvent>, IEventHandler<CompanyStockCreatedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public CompanyProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }
        public async Task HandleAsync(CompanyCreatedEvent @event)
        {
            var projection = new CompanyProjection(@event.AggregateId, @event.Name);
            await _projectionWriter.AddAsync(projection);
        }

        public async Task HandleAsync(CompanyStockCreatedEvent @event)
        {
            await _projectionWriter.UpdateAsync<CompanyProjection>(@event.AggregateId, projection =>
                {
                    projection.StockCode = @event.Code;
                    projection.StockQuantity = @event.Quantity;
                    projection.StockUnit = @event.Unit;
                });
        }
    }
}
