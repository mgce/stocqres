using System;
using System.Collections.Generic;
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
            await _context.Resolve<IEventHandler<TEvent>>().HandleAsync(@event);
        }
    }
}
