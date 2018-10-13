using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;

namespace Stocqres.Core.Domain
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        public object CreateAsync<T>(IEnumerable<IEvent> events)
        {
            var type = typeof(T);
            var ctor = type.GetConstructor(new[] {typeof(IEnumerable<IEvent>) });
            return ctor.Invoke(new object[] {events});
        }
    }
}
