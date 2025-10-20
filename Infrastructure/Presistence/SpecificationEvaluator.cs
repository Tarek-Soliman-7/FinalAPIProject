using Domain.Contracts;
using Domain.Entities;
using System.Linq.Expressions;

namespace Presistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery , 
            ISpecifications<TEntity,TKey> specifications ) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            if(specifications.OrderBy is not null)
                query= query.OrderBy(specifications.OrderBy);

            if(specifications.OrderByDescending is not null)
                query= query.OrderByDescending(specifications.OrderByDescending); 

            if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count>0)
                //foreach( var expression in specifications.IncludeExpressions )
                //    query=query.Include(expression);
                query = specifications.IncludeExpressions
                    .Aggregate(query, (currentQuery, expression) => currentQuery.Include(expression));
            
            if(specifications.IsPaginated)
            {
                query.Skip(specifications.Skip).Take(specifications.Take);
            }



            return query;
        }
    }
}
