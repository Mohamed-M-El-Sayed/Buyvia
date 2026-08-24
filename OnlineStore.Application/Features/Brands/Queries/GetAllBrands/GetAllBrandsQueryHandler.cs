using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Brands.Dtos;
using OnlineStore.Application.Features.Brands.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllBrandsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetAllBrandsQuery, IEnumerable<BrandDto>>
    {

        public async Task<IEnumerable<BrandDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all brands.");
            var brands = await unitOfWork.Repository<ProductBrand>()
                .GetAllWithSpecAsync(new GetAllBrandsSpecification(), cancellationToken);
            return mapper.Map<IEnumerable<BrandDto>>(brands);
        }
    }
}
