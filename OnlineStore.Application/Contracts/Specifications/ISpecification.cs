using System.Linq.Expressions;
using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Application.Contracts.Specifications
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>>? Criteria { get; set; }
        List<Expression<Func<TEntity, object>>> Includes { get; set; }
        List<string> IncludeStrings { get; set; }
        Expression<Func<TEntity, object>>? OrderBy { get; set; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; set; }
        bool IsTrackingEnabled { get; set; }
        int Skip { get; set; }
        int Take { set; get; }
        bool IsPaginationEnabled { get; set; }
        // if true then apply skip and take 
    }
}
