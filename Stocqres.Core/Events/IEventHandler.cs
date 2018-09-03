using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Stocqres.Core.Events
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> 
        where TEvent : IEvent
    {
    }
}
