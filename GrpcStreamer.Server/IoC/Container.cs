﻿using System;
using GrpcStreamer.Server.DataAccess;
using GrpcStreamer.Server.Infrastructure;
using GrpcStreamer.Server.Infrastructure.Configuration;
using GrpcStreamer.Server.Infrastructure.Mappers;
using GrpcStreamer.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Server.IoC
{
    public class Container : IContainer
    {
        private static IContainer container;

        public IConfiguration Configuration { get; private set; }
        private readonly ServiceProvider serviceProvider;

        private Container(IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            serviceProvider = services?.BuildServiceProvider() ?? throw new ArgumentNullException(nameof(services));
        }

        public static IContainer GetContainer()
        {
            if (container == null)
            {
                var configuration = GetConfiguration();

                var services = new ServiceCollection();

                RegisterServices(services, configuration);

                container = new Container(services, configuration);

                //ApplyMigrations<GrpcStreamerDbContext>();
            }

            return container;
        }

        public TService Resolve<TService>()
        {
            return serviceProvider.GetService<TService>();
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure.AddConsole());

            services.Configure<StreamerOptions>(configuration);
            services.AddSingleton<IItemMapper, ItemMapper>();
            services.AddSingleton<IStreamerServer, StreamerServer>();

            var connectionString = configuration.GetConnectionString("master");

            services.AddTransient<IConnectionFactory, ConnectionFactory>(c => new ConnectionFactory(connectionString));
            //services.AddDbContext<GrpcStreamerDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IStreamerService, StreamerService>();
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

        //private static void ApplyMigrations<TContext>() where TContext : DbContext
        //{
        //    using (var serviceScope = container.Resolve<IServiceScopeFactory>().CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();
        //        context.Database.Migrate();
        //    }
        //}
    }
}
