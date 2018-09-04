using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Stocqres.Core.Events
{
    public class EventBus : IEventBus
    {
        private readonly Mediator _mediator;

        public EventBus(Mediator mediator)
        {
            _mediator = mediator;
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return _mediator.Publish(@event);
        }
    }
}
