using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events;
using Stocqres.Domain.Events.Wallet;

namespace Stocqres.Application.Wallet.Handlers
{
    public class WalletEventHandler : IEventHandler<WalletCreatedEvent>, IEventHandler<WalletAmountDecreasedEvent>, IEventHandler<WalletAmountIncreasedEvent>
    {
        private readonly ICustomEventStore _eventStore;

        public WalletEventHandler(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }

        public async Task HandleAsync(WalletAmountDecreasedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }

        public async Task HandleAsync(WalletAmountIncreasedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }
    }
}
