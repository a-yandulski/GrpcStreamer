using System;
using GrpcStreamer.Client.Infrastructure.Configuration;
using GrpcStreamer.Server.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Client.IoC
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
            services.AddTransient<IStreamerClient, StreamerClient>();
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
    }
}
