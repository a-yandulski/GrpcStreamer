using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GrpcStreamer.Client.Helpers;
using GrpcStreamer.Client.Infrastructure;
using GrpcStreamer.Client.IoC;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Client
{
    class Program
    {
        private const int BatchSize = 5000;
        private static ILogger<Program> logger = Container.GetContainer().Resolve<ILogger<Program>>();

        static void Main(string[] args)
        {
            Console.WriteLine("GrpcStreamer.Client");
            Console.WriteLine("Press any key to start processing...");
            Console.ReadKey();

            using (var tokenSource = new CancellationTokenSource())
            {
                Task.Run(async () => await Run(BatchSize, 0, tokenSource.Token), tokenSource.Token);

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();

                tokenSource.Cancel();
            }
        }

        public static async Task Run(int top = 1000, int skip = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            var clientFactory = Container.GetContainer().Resolve<IClientFactory>();

            // Retain count of processed items for correct retries.
            var processedCount = new Ref<int>(skip);
            var @continue = false;

            Console.WriteLine("Processing records.");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var client = clientFactory.Create())
            {
                do
                {
                    // Process batch with retry
                    @continue = await RetryHelper.WithRetryAsync(async () => await client.Process(top, processedCount, cancellationToken), 3, logger);

                    Console.WriteLine("Completed processing a batch of records. Total count = {0}. Time (ms) = {1}.", processedCount.Value, stopwatch.ElapsedMilliseconds);

                } while (@continue);
            }
        }
    }
}
