namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        //GetAll
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking=false);
        //GetById
        Task<TEntity?> GetByIdAsync(TKey id);
        //Add
        Task AddAsync(TEntity entity);
        //Update
        void Update(TEntity entity);
        //Remove
        void Delete(TEntity entity);
        #region Specifications
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity,TKey> specifications);
        //GetById
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications);
        Task<int> CountAsync(ISpecifications<TEntity,TKey> specifications);
        #endregion
    }
}
