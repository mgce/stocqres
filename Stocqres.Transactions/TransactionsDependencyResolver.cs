using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;

namespace Stocqres.Transactions
{
    public static class TransactionsDependencyResolver
    {
        public static void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a=>a.FullName.Contains("Stocqres")).ToArray();


            //builder
            //    .RegisterAssemblyTypes(assemblies)
            //    .AsClosedTypesOf(typeof(ICommandHandler<>))
            //    .InstancePerLifetimeScope();

            //builder
            //    .RegisterAssemblyTypes(assemblies)
            //    .AsClosedTypesOf(typeof(IEventHandler<>))
            //    .InstancePerLifetimeScope();

        }
    }
}
