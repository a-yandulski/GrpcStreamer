using System.Data;

namespace GrpcStreamer.Server.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}