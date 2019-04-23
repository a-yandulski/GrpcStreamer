using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcStreamer.Client
{
    public interface IStreamerClient : IDisposable
    {
        Task<bool> Process(int top, Ref<int> processedCount, CancellationToken cancellationToken = default(CancellationToken));
    }
}