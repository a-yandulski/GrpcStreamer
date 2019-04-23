namespace GrpcStreamer.Client.Infrastructure
{
    public interface IClientFactory
    {
        IStreamerClient Create();
    }
}