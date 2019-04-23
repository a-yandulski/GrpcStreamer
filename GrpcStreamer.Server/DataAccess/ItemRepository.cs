using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using GrpcStreamer.Server.Domain;
using GrpcStreamer.Server.Infrastructure;

namespace GrpcStreamer.Server.DataAccess
{
    public class ItemRepository : IItemRepository
    {
        private readonly IConnectionFactory connectionFactory;
        private const string ListAllQueryFormat = "SELECT ItemId, [Value], StatusId FROM dbo.TItem ORDER BY ItemId OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY";

        public ItemRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public IEnumerable<Item> ListAll(int top, int skip)
        {
            var command = new CommandDefinition(string.Format(ListAllQueryFormat, skip, top));

            using (var connection = connectionFactory.Create())
            using (var reader = connection.ExecuteReader(command, CommandBehavior.SequentialAccess))
            {
                var values = new object[3];

                while (reader.Read())
                {
                    reader.GetValues(values);

                    yield return new Item
                    {
                        ItemId = Convert.ToInt32(values[0]),
                        Value = Convert.ToString(values[1]),
                        StatusId = Convert.ToInt32(values[2])
                    };
                }
            }
        }

        public async Task UpdateAsync(Item item)
        {
            using (var connection = connectionFactory.Create())
            {
                await connection.UpdateAsync(item);
            }
        }
    }
}
