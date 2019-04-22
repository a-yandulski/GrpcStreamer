using Dapper.Contrib.Extensions;

namespace GrpcStreamer.Server.Domain
{
    [Table("TItem")]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public string Value { get; set; }

        public int StatusId { get; set; }
    }
}
