using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Stocqres.Core.Mongo
{
    public static class Extensions
    {
        public static void ConfigureMongo(this ContainerBuilder builder)
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

            builder.Register(context =>
            {
                var mongoClient = context.Resolve<MongoClient>();
                return mongoClient.StartSession();
            }).SingleInstance();
        }
    }
}
