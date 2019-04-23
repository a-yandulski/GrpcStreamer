using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrpcStreamer.Server.DataAccess;
using GrpcStreamer.Server.Domain;
using Microsoft.Extensions.Logging;

namespace GrpcStreamer.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository itemRepository;
        private readonly ILogger<ItemService> logger;

        public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger)
        {
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Item> ListAll(int top = 1000, int skip = 0)
        {
            try
            {
                return itemRepository.ListAll(top, skip);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred.");
                throw;
            }
        }

        public async Task Update(Item item)
        {
            try
            {
                await itemRepository.UpdateAsync(item);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred.");
                throw;
            }
        }
    }
}
