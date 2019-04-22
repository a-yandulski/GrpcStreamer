using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GrpcStreamer.Server.DataAccess
{
    public class GrpcStreamerDbContextFactory : IDesignTimeDbContextFactory<GrpcStreamerDbContext>
    {
        public GrpcStreamerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GrpcStreamerDbContext>();
            optionsBuilder.UseSqlServer("server=(local);database=GrpcStreamer;Integrated Security=sspi;");

            return new GrpcStreamerDbContext(optionsBuilder.Options);
        }
    }
}
