using Autofac;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.Projections;
using Stocqres.Infrastructure.UnitOfWork;

namespace Stocqres.Infrastructure
{
    public static class InfrastructureDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectionWriter>().As<IProjectionWriter>();
            builder.RegisterType<ProjectionReader>().As<IProjectionReader>();
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<DatabaseProvider.DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();
        }
    }
}
