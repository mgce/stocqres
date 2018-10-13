using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;

namespace Stocqres.Core.Domain
{
    public interface IAggregateRootFactory
    {
        object CreateAsync<T>(IEnumerable<IEvent> events);
    }
}
