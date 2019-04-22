using System;
using System.Threading.Tasks;

namespace GrpcStreamer.Client
{
    public interface IStreamerClient : IDisposable
    {
        Task ProcessItems(int top = 1000, int skip = 0);
    }
}