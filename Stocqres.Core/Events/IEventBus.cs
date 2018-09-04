using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocqres.Core.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
