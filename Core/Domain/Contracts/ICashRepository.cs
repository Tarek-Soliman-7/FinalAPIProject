namespace Domain.Contracts
{
    public interface ICashRepository
    {
        //Get
        Task<string?> GetAsync(string cashKey);
        //Set
        Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive);
    }
}
