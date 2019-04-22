using System.Collections.Generic;
using System.Threading.Tasks;
using GrpcStreamer.Server.Domain;

namespace GrpcStreamer.Server.Services
{
    public interface IItemService
    {
        IEnumerable<Item> ListAll(int top = 1000, int skip = 0);
        Task Update(Item item);
    }
}