﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NJsonSchema;
using NSwag.AspNetCore;
using Stocqres.Core;
using Stocqres.Core.Authentication;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Events;
using Stocqres.Core.Middlewares;
using Stocqres.Core.Mongo;
using Stocqres.Customers;
using Stocqres.Identity;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Identity.Repositories;
using Stocqres.Infrastructure;
using Stocqres.Infrastructure.Commands;
using Stocqres.Infrastructure.ExternalServices.StockExchangeService;
using Stocqres.SharedKernel;
using Stocqres.Transactions;
using Stocqres.Transactions.Infrastructure.ProcessManagers;
using Stocqres.Transactions.Orders.Domain;

namespace Stocqres.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwagger();
            services.AddJwt();

            services.AddDbContext<IdentityDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddDbContext<ProcessManagerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));

            services.AddHttpClient<IStockExchangeService, StockExchangeService>(client =>
            {
                var config = Configuration.GetSection("StockExchange");
                client.BaseAddress = new Uri(config.GetValue<string>("BaseAddress"));
            });

            return AddAutofac(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
            });
            app.UseAuthentication();
            app.UseErrorHandler();
            app.UseMvc();

            var dispatcher = ApplicationContainer.Resolve<IDispatcher>();
            var config = ApplicationContainer.Resolve<IConfiguration>();
            var seeder = new Seeder(dispatcher, config);
            seeder.Seed();
        }

        private IServiceProvider AddAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            CoreDependencyContainer.Load(builder);
            InfrastructureDependencyContainer.Load(builder);
            IdentityDependencyContainer.Load(builder);
            CustomerDependencyContainer.Load(builder);
            SharedKernelDependencyResolver.Load(builder);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypesSafely()).Where(a => a.Namespace != null && a.Namespace.Contains("Stocqres")).ToArray();

            builder.ConfigureCqrs(assemblies);
            builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>),
                "commandHandler");
            builder.Populate(services);
            builder.ConfigureMongo();
            RegisterRepositories(builder);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }

    internal static class AssemblyExtensions
    {
        public static Type[] GetTypesSafely(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types
                    .Where(t => t != null)
                    .ToArray();
            }
        }
    }
}
