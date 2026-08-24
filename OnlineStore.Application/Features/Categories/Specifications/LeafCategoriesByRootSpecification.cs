using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Specifications
{
    public class LeafCategoriesByRootSpecification : BaseSpecification<ProductCategory>
    {
        public LeafCategoriesByRootSpecification(int rootId)
        {
            Criteria = c => c.Parent != null && c.Parent.ParentId == rootId;
        }
    }
}
