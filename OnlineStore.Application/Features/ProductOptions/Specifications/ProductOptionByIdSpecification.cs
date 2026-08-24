using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Specifications
{
    public class ProductOptionByIdSpecification : BaseSpecification<ProductOption>
    {
        public ProductOptionByIdSpecification(int optionId)
            : base(o => o.Id == optionId)
        {
            ApplyInclude(o => o.Values);
        }
    }
}
