using Domain.Contracts;
using StackExchange.Redis;

namespace Presistence.Repositories
{
    public class CashRepository(IConnectionMultiplexer _connection) : ICashRepository
    {
        readonly IDatabase _database = _connection.GetDatabase(); 
        public async Task<string?> GetAsync(string cashKey)
        {
            var cashValue = await _database.StringGetAsync(cashKey);
            return cashValue.IsNullOrEmpty ? null : cashValue.ToString();
        }

        public async Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive)
        {
            await _database.StringSetAsync(cashKey, cashValue, timeToLive);
        }
    }
}
