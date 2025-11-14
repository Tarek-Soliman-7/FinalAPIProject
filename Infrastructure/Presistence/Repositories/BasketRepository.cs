using Domain.Contracts;
using Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
    {
        private readonly IDatabase _database=_connection.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var result= await _database.StringSetAsync(basket.Id, jsonBasket,timeToLive??TimeSpan.FromDays(30));
            return result ? await GetBasketAsync(basket.Id) : null;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        => await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
          var result = await _database.StringGetAsync(id);
          return result.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(result!); 
        }
    }
}
