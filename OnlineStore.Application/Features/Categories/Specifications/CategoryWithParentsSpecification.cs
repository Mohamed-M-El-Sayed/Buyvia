using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Specifications
{
    public class CategoryWithParentsSpecification : BaseSpecification<ProductCategory>
    {
        public CategoryWithParentsSpecification(int cateogtyId)
        {
            Criteria = category => category.Id == cateogtyId;
            ApplyInclude(category => category.Parent!);
            ApplyInclude("Parent.Parent");
        }

    }
}
