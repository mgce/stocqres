using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Stocqres.Core.Events;

namespace Stocqres.Core.EventStore
{
    public interface ICustomEventStore
    {
        Task AppendToStream(Guid streamId, IEvent @event,
            CancellationToken cancellationToken = default(CancellationToken));
        Task AppendToStream(IEvent @event, CancellationToken cancellationToken = default(CancellationToken));
        Task<T> Load<T>(Guid id) where T : class, new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> FindAsync<T>(Expression<Func<T, bool>> predicate);
    }
}
