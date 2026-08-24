using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Specifications
{
    public class ProductWithVariantsSpecification : BaseSpecification<Product>
    {
        public ProductWithVariantsSpecification(int productId)
        {
            Criteria = p => p.Id == productId;
            ApplyInclude(p => p.Variants);
        }
    }
}
