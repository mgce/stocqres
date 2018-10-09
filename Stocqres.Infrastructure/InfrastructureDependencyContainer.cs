using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Infrastructure.ProjectionWriter;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure
{
    public static class InfrastructureDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectionWriter.ProjectionWriter>().As<IProjectionWriter>();
        }
    }
}
