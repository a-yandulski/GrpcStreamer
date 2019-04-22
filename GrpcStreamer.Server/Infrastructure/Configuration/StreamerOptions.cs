using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcStreamer.Server.Infrastructure.Configuration
{
    public class StreamerOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
