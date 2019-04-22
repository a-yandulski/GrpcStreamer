using System;
using System.Data;
using System.Data.SqlClient;

namespace GrpcStreamer.Server.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public ConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(connectionString);
        }
    }
}
