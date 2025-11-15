namespace Services.Implementations
{
    public class CashService : ICashService
    {
        public Task<string?> GetAsync(string cashKey)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive)
        {
            throw new NotImplementedException();
        }
    }
}
