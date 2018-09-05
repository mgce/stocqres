using System;
using System.Threading;
using System.Threading.Tasks;
using Stocqres.Core.Events;

namespace Stocqres.Core.EventStore
{
    public interface ICustomEventStore
    {
        Task AppendToStream(Guid streamId, IEvent @event,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
