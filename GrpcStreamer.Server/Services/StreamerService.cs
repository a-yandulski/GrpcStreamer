using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcStreamer.Server.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using Streamer;
using StreamerBase = Streamer.Streamer.StreamerBase;

namespace GrpcStreamer.Server.Services
{
    public class StreamerService : StreamerBase, IStreamerService
    {
        private readonly IItemService itemService;
        private readonly IItemMapper mapper;
        private readonly ILogger<StreamerService> logger;

        public StreamerService(IItemService itemService, IItemMapper mapper, ILogger<StreamerService> logger)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task Send(IAsyncStreamReader<StreamerRequest> requestStream, IServerStreamWriter<StreamerResponse> responseStream, ServerCallContext context)
        {
            // Receive initial request
            var topHeader = context.RequestHeaders.First(x => x.Key == "x-top");
            var skipHeader = context.RequestHeaders.First(x => x.Key == "x-skip");

            var top = Convert.ToInt32(topHeader.Value);
            var skip = Convert.ToInt32(skipHeader.Value);

            logger.LogInformation("Received initial request. Starting to stream items: Take={0}, Skip={1}", top, skip);

            // Stream items to client
            foreach (var item in itemService.ListAll(top, skip))
            {
                logger.LogInformation("Sending item: Id={0}, Value={1}, StatusId={2}", item.ItemId, item.Value, item.StatusId);

                var response = mapper.ToContract(item);

                await responseStream.WriteAsync(response);

                // Wait for response
                if (!await requestStream.MoveNext(context.CancellationToken))
                {
                    return;
                }

                var request = requestStream.Current;

                logger.LogInformation("Received processing status: ItemId={0}, Status={1}", request.ItemId, request.ItemStatus.ToString());

                // Update the item and save changes
                item.StatusId = (int)mapper.ToDomain(request);

                logger.LogInformation("Persisting item: Id={0}, Value={1}, StatusId={2}", item.ItemId, item.Value, item.StatusId);

                await itemService.Update(item);
            }
        }
    }
}
