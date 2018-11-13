using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Stocqres.Core.Events;

namespace Stocqres.Core.Commands
{
    public static class Extensions
    {
        public static void ConfigureCqrs(this ContainerBuilder builder, Assembly[] assemblies)
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IEventHandler<>));

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.Register<Func<Type, IEnumerable<IEventHandler<IEvent>>>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return t =>
                {
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(t);
                    var handlersCollectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);
                    return (IEnumerable<IEventHandler<IEvent>>)ctx.Resolve(handlersCollectionType);
                };
            });
        }
    }
}
