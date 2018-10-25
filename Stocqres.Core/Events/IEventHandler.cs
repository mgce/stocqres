using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocqres.Core.Events
{
    public interface IEventHandler
    {

    }
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
