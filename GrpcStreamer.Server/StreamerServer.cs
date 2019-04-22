using System;
using Grpc.Core;
using GrpcStreamer.Server.Infrastructure.Configuration;
using GrpcStreamer.Server.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrpcStreamer.Server
{
    public class StreamerServer : IStreamerServer
    {
        private readonly ILogger<StreamerServer> logger;
        private readonly Grpc.Core.Server server;
        private readonly StreamerOptions options;

        public StreamerServer(IOptions<StreamerOptions> options, IStreamerService streamerService, ILogger<StreamerServer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (streamerService == null)
            {
                throw new ArgumentNullException(nameof(streamerService));
            }

            if (!(streamerService is Streamer.Streamer.StreamerBase impl))
            {
                throw new ArgumentException($"{streamerService.GetType().FullName} must derive from {typeof(Streamer.Streamer.StreamerBase).FullName}", nameof(streamerService));
            }

            logger.LogInformation("Registering server: Host={0}, Port={1}", this.options.Host, this.options.Port);

            server = new Grpc.Core.Server
            {
                Services = { Streamer.Streamer.BindService(impl) },
                Ports = { new ServerPort(this.options.Host, this.options.Port, ServerCredentials.Insecure) }
            };
        }

        public void Start()
        {
            logger.LogInformation("Starting server: Host={0}, Port={1}", options.Host, options.Port);

            server.Start();
        }

        public void Stop()
        {
            logger.LogInformation("Stopping server: Host={0}, Port={1}", options.Host, options.Port);

            server.ShutdownAsync().Wait();
        }
    }
}
