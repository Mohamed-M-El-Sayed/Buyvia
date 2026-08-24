using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Specifications
{
    public class CategoryWithSubCategoriesSpecification : BaseSpecification<ProductCategory>
    {
        public CategoryWithSubCategoriesSpecification(int categoryId)
        {
            Criteria = c => c.Id == categoryId;
            ApplyInclude(c => c.SubCategories);
        }
    }
}
