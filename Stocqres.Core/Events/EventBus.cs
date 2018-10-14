using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Stocqres.Core.Events
{
    public class EventBus : IEventBus
    {
        private readonly IComponentContext _context;

        public EventBus(IComponentContext context)
        {
            _context = context;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _context.Resolve<IEnumerable<IEventHandler<TEvent>>>().ToList();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
