using MediatR;
using OnlineStore.Application.Features.Brands.Dtos;

namespace OnlineStore.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQuery : IRequest<IEnumerable<BrandDto>>
    {
    }
}
