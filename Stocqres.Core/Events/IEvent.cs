
using System;

namespace Stocqres.Core.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
    }
}
