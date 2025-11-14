using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        //SaveChangesAsync
        Task<int> SaveChangesAsync();

        // Method return obj from GenericRepository of [Entity]
        IGenericRepository<TEntity,TKey> GetRepository<TEntity,TKey>() where TEntity : BaseEntity<TKey>;
    }
}
