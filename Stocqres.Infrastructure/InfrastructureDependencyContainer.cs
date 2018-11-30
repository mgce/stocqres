using Autofac;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventStore;
using Stocqres.Infrastructure.Projections;
using Stocqres.Infrastructure.Snapshots;
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
            builder.RegisterType<EventStore.EventStore>().As<IEventStore>().SingleInstance();
            builder.RegisterType<SnapshotService>().As<ISnapshotService>().SingleInstance();
        }
    }
}
