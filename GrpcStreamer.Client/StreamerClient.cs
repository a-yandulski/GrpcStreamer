using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcStreamer.Client.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrpcStreamer.Client
{
    public class StreamerClient : IStreamerClient
    {
        private readonly Channel channel;
        private readonly Streamer.Streamer.StreamerClient client;
        private readonly ILogger<StreamerClient> logger;
        private readonly StreamerOptions options;

        public StreamerClient(IOptions<StreamerOptions> options, ILogger<StreamerClient> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            logger.LogInformation("Registering client for server: Host={0}, Port={1}", this.options.Host, this.options.Port);

            channel = new Channel($"{this.options.Host}:{this.options.Port}", ChannelCredentials.Insecure);
            client = new Streamer.Streamer.StreamerClient(channel);
        }

        public async Task ProcessItems(int top = 1000, int skip = 0)
        {
            logger.LogInformation("Initiating connection to server: Host={0}, Port={1}", this.options.Host, this.options.Port);
            logger.LogInformation("Requesting {0} items starting from record position {1}", top, skip);

            var headers = new Metadata
            {
                 new Metadata.Entry("x-top", top.ToString()),
                 new Metadata.Entry("x-skip", skip.ToString()),
            };

            // Initiate connection and start bi-directional communication
            using (var connection = client.Send(headers))
            {
                while (await connection.ResponseStream.MoveNext())
                {
                    var response = connection.ResponseStream.Current;
                    var item = response.Payload;

                    if (item == null)
                    {
                        logger.LogInformation("No items received. Stopping processing.");

                        break;
                    }

                    logger.LogInformation("Received item: Id={0}, Value={0}", item.Id, item.Value);

                    var status = new Streamer.ItemStatus
                    {
                        ItemId = item.Id,
                        Value = Streamer.Status.Completed
                    };

                    logger.LogInformation("Sending processing result: ItemId={0}, Status={0}", status.ItemId, status.Value.ToString());

                    await connection.RequestStream.WriteAsync(new Streamer.StreamerRequest
                    {
                        Payload = status
                    });
                }

                await connection.RequestStream.CompleteAsync();
            }
        }

        public void Dispose()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}
