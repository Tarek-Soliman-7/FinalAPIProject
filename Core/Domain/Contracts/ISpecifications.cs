namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        //Signature for prop [Exp ==> Where]
        public Expression<Func<TEntity, bool>>? Criteria { get; }
        //Signature for prop [Exp ==> Include]
        // Include (p=>p.productType).Include(p=>p.productBrand)
        public List<Expression<Func<TEntity, object>>>? IncludeExpressions { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        //Pagination [Skip,Take] {ints}

        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }


    }
}
