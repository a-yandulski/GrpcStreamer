using System;
using Grpc.Core;
using GrpcStreamer.Client.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrpcStreamer.Client.Infrastructure
{
    public class ClientFactory : IClientFactory
    {
        private readonly ILogger<ClientFactory> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly StreamerOptions options;

        public ClientFactory(IOptions<StreamerOptions> options, ILogger<ClientFactory> logger, ILoggerFactory loggerFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IStreamerClient Create()
        {
            var channel = new Channel($"{this.options.Host}:{this.options.Port}", ChannelCredentials.Insecure);
            var client = new Streamer.Streamer.StreamerClient(channel);

            return new StreamerClient(client, channel, loggerFactory.CreateLogger<StreamerClient>());
        }
    }
}
