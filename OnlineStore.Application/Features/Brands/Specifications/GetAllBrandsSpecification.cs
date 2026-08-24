using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Specifications
{
    public class GetAllBrandsSpecification : BaseSpecification<ProductBrand>
    {
        public GetAllBrandsSpecification()
        {
            ApplyOrderBy(b => b.Name);
        }
    }
}
