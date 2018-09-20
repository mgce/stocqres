using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events.StockGroup;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Application.StockGroup.Handlers
{
    public class StockGroupEventHandler : IEventHandler<StockGroupCreatedEvent>, IEventHandler<StockGroupQuantityChangedEvent>
    {
        private readonly ICustomEventStore _eventStore;
        private readonly IStockGroupRepository _stockGroupRepository;

        public StockGroupEventHandler(ICustomEventStore eventStore, 
            IStockGroupRepository stockGroupRepository)
        {
            _eventStore = eventStore;
            _stockGroupRepository = stockGroupRepository;
        }
        public async Task HandleAsync(StockGroupCreatedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
            await _stockGroupRepository.CreateAsync(new Domain.StockGroup(@event.OwnerId, @event.StockOwner, @event.Quantity,
                @event.Price, @event.StockId));
        }

        public async Task HandleAsync(StockGroupQuantityChangedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
            var entity = await _stockGroupRepository.GetAsync(@event.Id);
            entity.UpdateQuantity(@event.Quantity);
            await _stockGroupRepository.UpdateAsync(entity);
        }
    }
}
