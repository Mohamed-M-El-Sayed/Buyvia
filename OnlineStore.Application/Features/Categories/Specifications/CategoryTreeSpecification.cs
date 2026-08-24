using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Specifications
{
    public class CategoryTreeSpecification : BaseSpecification<ProductCategory>
    {
        public CategoryTreeSpecification()
        {
            // categories with no parent 
            Criteria = category => category.ParentId == null;
            ApplyInclude(category => category.SubCategories);
            ApplyInclude("SubCategories.SubCategories");


        }
    }
}
