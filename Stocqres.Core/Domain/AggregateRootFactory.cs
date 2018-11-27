using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;

namespace Stocqres.Core.Domain
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        public object CreateAsync<T>(IEnumerable<IEvent> events)
        {
            if(events == null || !events.Any())
                throw new StocqresException("List of events cannot be null or empty");

            var type = typeof(T);
            //var recreateMethod = type.GetMethod("AggregateRoot", new[] {events.GetType()});
            var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance , null,new[] {typeof(IEnumerable<IEvent>) },null);
            //var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            //var ctor = typeof(T).GetConstructors(flags).Single(
            //    ctors =>
            //    {
            //        var parameters = ctors.GetParameters();
            //        return parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerable<IEvent>);
            //    });
            //var value = Expression.Parameter(typeof(IEnumerable<IEvent>), "events");
            //var body = Expression.New(ctor, value);
            //var lambda = Expression.Lambda<Func<IEnumerable<IEvent>, T>>(body, value);

            //return lambda.Compile();
            return ctor.Invoke(new object[] {events});
            
        }
    }
}
