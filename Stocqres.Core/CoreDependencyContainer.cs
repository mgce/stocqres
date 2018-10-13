using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;

namespace Stocqres.Core
{
    public static class CoreDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandBus>().As<ICommandBus>();
            builder.RegisterType<EventBus>().As<IEventBus>();
            builder.RegisterType<AggregateRootFactory>().As<IAggregateRootFactory>();
            builder.AddDispatchers();
        }
    }
}
