using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Contracts.Specifications;
using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Infrastructure.Persistence.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {

        public static IQueryable<TEntity> GenerateQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;
            if (specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            if (specification.Criteria is not null && specification.Includes.Any())
            {
                foreach (var include in specification.Includes)
                    query = query.Include(include);
            }
            if (specification.IncludeStrings.Any())
            {
                foreach (var includeString in specification.IncludeStrings)
                    query = query.Include(includeString);
            }
            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);
            if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);
            if (specification.IsPaginationEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);
            if (!specification.IsTrackingEnabled)
                query = query.AsNoTracking();
            return query;
        }
    }
}
