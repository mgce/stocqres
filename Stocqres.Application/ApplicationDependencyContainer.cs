using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Application.StockExchange.Services;
using Stocqres.Application.StockGroup.Services;
using Stocqres.Application.Token.Services;
using Stocqres.Application.User.Handlers;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;

namespace Stocqres.Application
{
    public static class ApplicationDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>();
            builder.RegisterType<StockExchangeService>().As<IStockExchangeService>();
            builder.RegisterType<StockGroupService>().As<IStockGroupService>();
        }
    }
}
