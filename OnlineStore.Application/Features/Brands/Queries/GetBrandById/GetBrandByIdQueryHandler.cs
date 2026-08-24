using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Brands.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBrandByIdQuery, BrandDto>
    {
        public async Task<BrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await unitOfWork.Repository<ProductBrand>()
                            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(ProductBrand), request.Id.ToString());
            var brandDto = mapper.Map<BrandDto>(brand);
            return brandDto;
        }
    }
}
