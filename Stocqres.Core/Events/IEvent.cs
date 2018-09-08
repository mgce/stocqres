
using System;

namespace Stocqres.Core.Events
{
    public interface IEvent
    {
        public Guid Id { get; set; }
    }
}
