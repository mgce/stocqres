using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Core.Queries;

namespace Stocqres.Core.Dispatcher
{
    public static class Extensions
    {
        public static void AddDispatchers(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandBus>().As<ICommandBus>();
            builder.RegisterType<Dispatcher>().As<IDispatcher>();
            builder.RegisterType<QueryBus>().As<IQueryBus>();
        }
    }
}
