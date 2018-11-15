using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel
{
    public static class SharedKernelDependencyResolver
    {
        public static void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Stocqres.SharedKernel")).ToArray();

            builder
               .RegisterAssemblyTypes(assemblies)
               .AsClosedTypesOf(typeof(IEventHandler<>))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
