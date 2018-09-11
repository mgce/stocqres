using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events.StockGroup;

namespace Stocqres.Application.StockGroup.Handlers
{
    public class StockGroupEventHandler : IEventHandler<StockGroupCreatedEvent>, IEventHandler<StockGroupQuantityChangedEvent>
    {
        private readonly ICustomEventStore _eventStore;

        public StockGroupEventHandler(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task HandleAsync(StockGroupCreatedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }

        public async Task HandleAsync(StockGroupQuantityChangedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }
    }
}
