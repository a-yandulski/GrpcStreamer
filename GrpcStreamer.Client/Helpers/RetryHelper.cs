using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Client.Helpers
{
    public static class RetryHelper
    {
        public static async Task<T> WithRetryAsync<T>(Func<Task<T>> action, int tries, ILogger logger)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var attempt = 1;

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception e)
                {
                    if (attempt == tries)
                    {
                        logger.LogError(e, "An error occurred receiving data from server. Attempt {0} of {1}.", attempt, tries);

                        throw;
                    }

                    logger.LogWarning("An error occurred receiving data from server. Attempt {0} of {1}.", attempt, tries);

                    attempt++;
                }
            }
        }
    }
}
