using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;
using System.Collections.Concurrent;


namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private ConcurrentDictionary<string, object> _repositories;
        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>

        //Dectionary ==> Collection [Key , Value]
        //Key : Name Of Entity [Product] ==> String
        // Value : Obj of GenericRepository
        //Product : new GenericRepository<Product,int>()
        //Dictionary<string,object>
        /// Way 1
        //var key=typeof(TEntity).Name;
        //if(! _repositories.ContainsKey(key))
        //    _repositories[key]= new GenericRepository<TEntity,TKey>(_dbContext);
        //return (IGenericRepository<TEntity, TKey>) _repositories[key];
        /// Way 2
        => (IGenericRepository<TEntity, TKey>)
            _repositories.GetOrAdd(typeof(TEntity).Name,
                (_) => new GenericRepository<TEntity, TKey>(_dbContext));
        

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
