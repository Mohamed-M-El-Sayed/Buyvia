using System.Linq.Expressions;
using OnlineStore.Application.Contracts.Specifications;
using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Application.Common.Specifications
{
    public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();
        public List<string> IncludeStrings { get; set; } = new();

        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }
        public bool IsTrackingEnabled { get; set; } = true;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecification(Expression<Func<TEntity, bool>> criteria) => Criteria = criteria;

        public BaseSpecification() { }

        public void ApplyInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        public void ApplyInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        public void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        public void ApplyOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        public void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginationEnabled = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        public void AsNoTracking()
        {
            IsTrackingEnabled = false;
        }

    }
}
