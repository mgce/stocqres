using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stocqres.Core.Events;

namespace Stocqres.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
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
            app.UseMvc();
        }

        private IServiceProvider AddAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            RegisterMarten(builder);
            return new AutofacServiceProvider(this.ApplicationContainer);
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
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => eventType.IsAssignableFrom(p));
            foreach (var type in types)
            {
                options.Events.AddEventType(type);
            }
        }
    }
}
