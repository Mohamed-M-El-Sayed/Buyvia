using MediatR;
using OnlineStore.Application.Features.Brands.Dtos;

namespace OnlineStore.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQuery(int id) : IRequest<BrandDto>
    {
        public int Id { get; } = id;
    }
}
