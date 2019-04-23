namespace GrpcStreamer.Server.Infrastructure.Mappers
{
    public interface IItemMapper
    {
        Streamer.StreamerResponse ToContract(Domain.Item item);

        Domain.ItemStatus ToDomain(Streamer.StreamerRequest request);
    }
}