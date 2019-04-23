using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Client
{
    public class StreamerClient : IStreamerClient
    {
        private readonly ILogger<StreamerClient> logger;
        private readonly Streamer.Streamer.StreamerClient client;
        private readonly Channel channel;

        public StreamerClient(Streamer.Streamer.StreamerClient client, Channel channel, ILogger<StreamerClient> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.channel = channel ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Process(int top, Ref<int> processedCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (processedCount == null)
            {
                throw new ArgumentNullException(nameof(processedCount));
            }

            var skip = (int) processedCount;

            logger.LogInformation("Initiating connection to server");

            logger.LogInformation("Requesting {0} items starting from position {1}", top, skip);

            var headers = new Metadata
                {
                    new Metadata.Entry("x-top", top.ToString()),
                    new Metadata.Entry("x-skip", skip.ToString()),
                };

            // Initiate connection and start bi-directional communication
            using (var call = client.Send(headers, cancellationToken: cancellationToken))
            {
                while (await call.ResponseStream.MoveNext(cancellationToken))
                {
                    var response = call.ResponseStream.Current;

                    if (response == null)
                    {
                        logger.LogInformation("No items received. Stopping processing.");

                        break;
                    }

                    //logger.LogInformation("Received item: Id={0}, Value={1}", response.Id, response.Value);

                    var request = new Streamer.StreamerRequest
                    {
                        ItemId = response.Id,
                        ItemStatus = Streamer.Status.Completed
                    };

                    //logger.LogInformation("Sending processing result: ItemId={0}, Status={1}", request.ItemId, request.ItemStatus.ToString());

                    await call.RequestStream.WriteAsync(request);

                    processedCount.Value++;
                }

                await call.RequestStream.CompleteAsync();

                return processedCount != skip;
            }
        }

        public void Dispose()
        {
            channel.ShutdownAsync().Wait();
        }

    }
}
