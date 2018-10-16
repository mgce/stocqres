
using System;

namespace Stocqres.Core.Events
{
    /// <summary>
    /// Represents an event message.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the identifier of the source originating the event.
        /// </summary>
        Guid AggregateId { get; set; }
    }
}
