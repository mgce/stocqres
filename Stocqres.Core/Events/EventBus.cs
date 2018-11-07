using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Stocqres.Core.Events
{
    public class EventBus : IEventBus
    {
        private readonly IComponentContext _context;
        private readonly Func<Type, IEnumerable<IEventHandler<IEvent>>> _handlersFactory;

        public EventBus(IComponentContext context, Func<Type, IEnumerable<IEventHandler<IEvent>>> handlersFactory)
        {
            _context = context;
            _handlersFactory = handlersFactory;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlerCollectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            var eventHandlers = _context.Resolve(handlerCollectionType);

            var hadnlers = new List<Task>();

            foreach (var handler in (IEnumerable) eventHandlers)
            {
                var handlerMethod = handler.GetType().GetMethod("HandleAsync", new[] {@event.GetType()});
                hadnlers.Add((Task) (handlerMethod.Invoke(handler, new object[] {@event})));
            }

            await Task.WhenAll(hadnlers);
        }
    }
}
