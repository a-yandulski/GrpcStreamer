namespace GrpcStreamer.Server.IoC
{
    public interface IContainer
    {
        TService Resolve<TService>();
    }
}