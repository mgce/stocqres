using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Stocqres.Core.Exceptions;
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

        public async Task<T> Load<T>(Guid id) where T : class, new()
        {
            try
            {
                return await _eventStore.AggregateStreamAsync<T>(id);
            }
            catch (Exception)
            {
                throw new StocqresException($"{typeof(T)} doesn't exist");
            }
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _eventStore.QueryRawEventDataOnly<T>().SingleOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw new StocqresException($"{typeof(T)} doesn't exist");
            }
        }

        public async Task<IReadOnlyList<T>> FindAsync<T>(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _eventStore.QueryRawEventDataOnly<T>().Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                throw new StocqresException($"{typeof(T)} doesn't exist");
            }
        }
    }
}
