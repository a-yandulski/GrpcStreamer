using System;
using Streamer;
using ItemStatus = GrpcStreamer.Server.Domain.ItemStatus;

namespace GrpcStreamer.Server.Infrastructure.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public Streamer.StreamerResponse ToContract(Domain.Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new Streamer.StreamerResponse
            {
                Id = item.ItemId,
                Value = item.Value
            };
        }

        public Domain.ItemStatus ToDomain(Streamer.StreamerRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            switch (request.ItemStatus)
            {
                case Status.Completed:
                    return ItemStatus.Completed;
                case Status.Failed:
                    return ItemStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.ItemStatus), request.ItemStatus, null);
            }
        }
    }
}
