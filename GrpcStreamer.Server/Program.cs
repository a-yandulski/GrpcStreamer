using System;
using GrpcStreamer.Server.IoC;

namespace GrpcStreamer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = Container.GetContainer().Resolve<IStreamerServer>();

            server.Start();

            Console.WriteLine("GrpcStreamer.Server is running.");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.Stop();
        }
    }
}
