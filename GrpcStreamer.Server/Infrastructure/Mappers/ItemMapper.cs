using System;
using Streamer;
using ItemStatus = GrpcStreamer.Server.Domain.ItemStatus;

namespace GrpcStreamer.Server.Infrastructure.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public Streamer.Item ToContract(Domain.Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new Streamer.Item
            {
                Id = item.ItemId,
                Value = item.Value
            };
        }

        public Domain.ItemStatus ToDomain(Streamer.ItemStatus status)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            switch (status.Value)
            {
                case Status.Completed:
                    return ItemStatus.Completed;
                case Status.Failed:
                    return ItemStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
