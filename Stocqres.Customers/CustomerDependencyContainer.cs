﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;

namespace Stocqres.Customers
{
    public static class CustomerDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //builder
            //    .RegisterAssemblyTypes(assembly)
            //    .AsClosedTypesOf(typeof(ICommandHandler<>));

            //builder
            //    .RegisterAssemblyTypes(assembly)
            //    .AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}
