using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Specifications
{
    public class ProductOptionsByProductIdSpecification : BaseSpecification<Product>
    {
        public ProductOptionsByProductIdSpecification(int productId)
           : base(p => p.Id == productId)
        {
            ApplyInclude(p => p.Options);
        }
    }
}
