using System.Threading.Tasks;
using Grpc.Core;
using Streamer;

namespace GrpcStreamer.Server.Services
{
    public interface IStreamerService
    {
        Task Send(IAsyncStreamReader<StreamerRequest> requestStream, IServerStreamWriter<StreamerResponse> responseStream, ServerCallContext context);
    }
}