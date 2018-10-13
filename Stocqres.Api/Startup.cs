using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
using Stocqres.Core;
using Stocqres.Core.Authentication;
using Stocqres.Core.Events;
using Stocqres.Core.Middlewares;
using Stocqres.Core.Mongo;
using Stocqres.Customers;
using Stocqres.Domain.Events.Users;
using Stocqres.Identity;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Infrastructure;
using Stocqres.Identity.Repositories;
using Stocqres.Infrastructure;
using Stocqres.Infrastructure.ExternalServices.StockExchangeService;
using Stocqres.Transactions;

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
            services.AddJwt();
            services.AddDbContext<IdentityDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddHttpClient<IStockExchangeService, StockExchangeService>(client =>
            {
                var config = Configuration.GetSection("StockExchangeCodes");
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
            app.UseAuthentication();
            app.UseErrorHandler();
            app.UseMvc();
            SeedData.Initialize(ApplicationContainer);
        }

        private IServiceProvider AddAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();

            //ApplicationDependencyContainer.Load(builder);
            CustomerDependencyContainer.Load(builder);
            IdentityDependencyContainer.Load(builder);
            TransactionsDependencyResolver.Load(builder);
            InfrastructureDependencyContainer.Load(builder);
            CoreDependencyContainer.Load(builder);

            builder.Populate(services);

            //RegisterJwt(ref services, builder);
            RegisterMongo(builder);
            RegisterRepositories(builder);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        //private void RegisterMarten(ContainerBuilder builder)
        //{
        //    builder.Register(add =>
        //    {
        //        var documentStore = DocumentStore.For(options =>
        //        {
        //            var config = Configuration.GetSection("EventStore");
        //            var connectionString = config.GetValue<string>("ConnectionString");
        //            var schemaName = config.GetValue<string>("Schema");

        //            options.Connection(connectionString);
        //            options.AutoCreateSchemaObjects = AutoCreate.All;
        //            options.Events.DatabaseSchemaName = schemaName;
        //            options.DatabaseSchemaName = schemaName;
        //            RegisterEvents(options);
        //        });
        //        return documentStore.OpenSession();
        //    }).InstancePerLifetimeScope();
        //}

        //private void RegisterEvents(StoreOptions options)
        //{
        //    var eventType = typeof(IEvent);
        //    var assembly = typeof(UserCreatedEvent).Assembly;
        //    var types = assembly.GetTypes().Where(p => eventType.IsAssignableFrom(p));
        //    foreach (var type in types)
        //    {
        //        options.Events.AddEventType(type);
        //    }
        //}

        private void RegisterJwt(ref IServiceCollection services, ContainerBuilder builder)
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

            builder.RegisterInstance(options);
            builder.RegisterType<JwtHandler>().As<IJwtHandler>().SingleInstance();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                    ValidAudience = options.ValidAudience,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = options.ValidateAudience,
                    ValidateLifetime = options.ValidateLifetime,
                    NameClaimType = JwtRegisteredClaimNames.Sub,
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
