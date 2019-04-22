namespace GrpcStreamer.Server.Infrastructure.Mappers
{
    public interface IItemMapper
    {
        Streamer.Item ToContract(Domain.Item item);

        Domain.ItemStatus ToDomain(Streamer.ItemStatus status);
    }
}