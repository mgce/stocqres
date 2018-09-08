using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events.StockGroup;

namespace Stocqres.Application.Stock.Handlers
{
    public class StockEventHandler : IEventHandler<StockGroupCreatedEvent>, IEventHandler<StockGroupQuantityChangedEvent>
    {
        private readonly ICustomEventStore _eventStore;

        public StockEventHandler(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task HandleAsync(StockGroupCreatedEvent @event)
        {
            await _eventStore.AppendToStream(@event);
        }

        public async Task HandleAsync(StockGroupQuantityChangedEvent @event)
        {
            await _eventStore.AppendToStream(@event);
        }
    }
}
