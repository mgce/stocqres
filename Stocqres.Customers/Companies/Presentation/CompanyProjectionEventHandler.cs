using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Customers.Companies.Domain.Events;
using Stocqres.Customers.Companies.Presentation.Projections;
using Stocqres.Infrastructure.ProjectionWriter;
using Stocqres.SharedKernel.Events;

namespace Stocqres.Customers.Companies.Presentation
{
    public class CompanyProjectionEventHandler : 
        IEventHandler<CompanyCreatedEvent>, 
        IEventHandler<CompanyStockCreatedEvent>, 
        IEventHandler<CompanyChargedEvent>
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

        public async Task HandleAsync(CompanyChargedEvent @event)
        {
            await _projectionWriter.UpdateAsync<CompanyProjection>(@event.AggregateId, projection =>
            {
                projection.StockQuantity = @event.StockQuantity;
            });
        }
    }
}
