using System;
using GrpcStreamer.Client.IoC;

namespace GrpcStreamer.Client
{
    class Program
    {
        private const int BatchSize = 5000;
        private const int TotalCount = 1000000; //Assuming we know how many records we have to update

        static void Main(string[] args)
        {
            Console.WriteLine("GrpcStreamer.Client");
            Console.WriteLine("Press any key to start processing.");
            Console.ReadKey();

            using (var client = Container.GetContainer().Resolve<IStreamerClient>())
            {
                Console.WriteLine("GrpcStreamer.Client is running.");
                Console.WriteLine($"Processing {TotalCount} record in batches by {BatchSize} records.");

                var total = 0;

                while (total < TotalCount)
                {
                    client.ProcessItems(BatchSize, total).Wait();

                    total += BatchSize;
                }
            }

            Console.WriteLine("GrpcStreamer.Client has finished processing.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
