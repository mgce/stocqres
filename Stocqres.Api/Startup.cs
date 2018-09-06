using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Stocqres.Application;
using Stocqres.Application.User.Handlers;
using Stocqres.Core;
using Stocqres.Core.Authentication;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Core.Mongo;
using Stocqres.Domain;
using Stocqres.Domain.Commands.User;
using Stocqres.Domain.Events.Users;
using Stocqres.Infrastructure;
using StoreOptions = Marten.StoreOptions;

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
            app.UseMvc();
        }

        private IServiceProvider AddAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();

            ApplicationDependencyContainer.Load(builder);
            InfrastructureDependencyContainer.Load(builder);
            CoreDependencyContainer.Load(builder);

            builder.Populate(services);

            RegisterMarten(builder);
            RegisterJwt(services, builder);
            RegisterMongo(builder);
            RegisterRepositories(builder);

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        private void RegisterMarten(ContainerBuilder builder)
        {
            builder.Register(add =>
            {
                var documentStore = DocumentStore.For(options =>
                {
                    var config = Configuration.GetSection("EventStore");
                    var connectionString = config.GetValue<string>("ConnectionString");
                    var schemaName = config.GetValue<string>("Schema");

                    options.Connection(connectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = schemaName;
                    options.DatabaseSchemaName = schemaName;
                    RegisterEvents(options);
                });
                return documentStore.OpenSession();
            }).InstancePerLifetimeScope();
        }

        private void RegisterEvents(StoreOptions options)
        {
            var eventType = typeof(IEvent);
            var assembly = typeof(UserCreatedEvent).Assembly;
            var types = assembly.GetTypes().Where(p => eventType.IsAssignableFrom(p));
            //var types = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => eventType.IsAssignableFrom(p));
            foreach (var type in types)
            {
                options.Events.AddEventType(type);
            }
        }

        private void RegisterJwt(IServiceCollection services, ContainerBuilder builder)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var options = new JwtOptions
            {
                SecretKey = jwtSection.GetValue<string>("SecretKey"),
                ValidAudience = jwtSection.GetValue<string>("ValidAudience"),
                Issuer = jwtSection.GetValue<string>("ValidIssuer"),
                ValidateAudience = jwtSection.GetValue<bool>("ValidateAudience"),
                ValidateLifetime = jwtSection.GetValue<bool>("ValidateLifetime"),
                ExpiryMinutes = jwtSection.GetValue<int>("ExpiryMinutes"),
            };

            builder.Register(r => options).SingleInstance();
            builder.RegisterType<JwtHandler>().As<IJwtHandler>().SingleInstance();

            services.AddAuthentication().AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                    ValidAudience = options.ValidAudience,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = options.ValidateAudience,
                    ValidateLifetime = options.ValidateLifetime
                };
            });
        }

        private void RegisterMongo(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var mongoOptions = new MongoOptions();
                configuration.GetSection("mongo").Bind(mongoOptions);

                return mongoOptions;
            }).SingleInstance();

            builder.Register(context =>
            {
                var mongoOptions = context.Resolve<MongoOptions>();
                return new MongoClient(mongoOptions.ConnectionString);
            }).SingleInstance();

            builder.Register(context =>
            {
                var mongoOptions = context.Resolve<MongoOptions>();
                var mongoClient = context.Resolve<MongoClient>();
                return mongoClient.GetDatabase(mongoOptions.Database);
            }).InstancePerLifetimeScope();
        }

        public void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
