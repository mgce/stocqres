using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Stocqres.Core.Events;

namespace Stocqres.Core.Commands
{
    public static class Extensions
    {
        public static void ConfigureCqrs(this ContainerBuilder builder, Type[] assemblies)
        {
            builder
                .RegisterTypes(assemblies)
                .AsClosedTypesOf(typeof(IEventHandler<>));

            //builder
            //    .RegisterTypes(assemblies)
            //    .AsClosedTypesOf(typeof(ICommandHandler<>));

            var commandHandlers = assemblies.Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>))).ToArray();

            builder.RegisterTypes(commandHandlers).As(type =>
                    type.GetInterfaces().Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(ICommandHandler<>)))
                        .Select(interfaceType => new KeyedService("commandHandler", interfaceType)))
                .InstancePerLifetimeScope();


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
