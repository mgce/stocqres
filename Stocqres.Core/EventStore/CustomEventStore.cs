using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using IEvent = Stocqres.Core.Events.IEvent;

namespace Stocqres.Core.EventStore
{
    public class CustomEventStore : ICustomEventStore
    {
        private IEventStore _eventStore => _session.Events;
        private readonly IDocumentSession _session;

        public CustomEventStore(IDocumentSession session)
        {
            _session = session;
        }

        public Task AppendToStream(Guid streamId, IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            _eventStore.Append(streamId, @event);
            return _session.SaveChangesAsync(cancellationToken);
        }

        public Task AppendToStream(IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            _eventStore.Append(@event.Id, @event);
            return _session.SaveChangesAsync(cancellationToken);
        }
    }
}
