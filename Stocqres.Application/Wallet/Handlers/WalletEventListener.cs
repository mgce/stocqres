using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events;

namespace Stocqres.Application.Wallet.Handlers
{
    public class WalletEventListener : IEventHandler<WalletCreatedEvent>
    {
        private readonly ICustomEventStore _eventStore;

        public WalletEventListener(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            await _eventStore.AppendToStream(@event.Id, @event);
        }
    }
}
