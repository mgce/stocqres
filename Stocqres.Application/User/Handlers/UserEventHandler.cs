using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Events.Users;

namespace Stocqres.Application.User.Handlers
{
    public class UserEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly ICustomEventStore _eventStore;

        public UserEventHandler(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _eventStore.AppendToStream(@event.Id, @event, cancellationToken);
        }
    }
}
