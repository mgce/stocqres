
using System;

namespace Stocqres.Core.Events
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
    }
}
