using System.Collections.Generic;
using System.Threading.Tasks;
using GrpcStreamer.Server.Domain;

namespace GrpcStreamer.Server.DataAccess
{
    public interface IItemRepository
    {
        IEnumerable<Item> ListAll(int top, int skip);
        Task UpdateAsync(Item item);
    }
}