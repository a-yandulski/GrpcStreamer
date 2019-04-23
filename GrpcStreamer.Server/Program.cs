using System;
using System.Threading;
using GrpcStreamer.Server.IoC;

namespace GrpcStreamer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            var server = Container.GetContainer().Resolve<IStreamerServer>();

            server.Start();

            Console.WriteLine("GrpcStreamer.Server is running.");
            Console.WriteLine("Press Ctrl+C to stop the server...");

            exitEvent.WaitOne();

            server.Stop();
        }
    }
}
