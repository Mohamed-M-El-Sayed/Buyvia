using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Dtos;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Queries.GetVariantById
{
    public class GetVariantByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetVariantByIdQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetVariantByIdQuery, ProductVariantDto>
    {
        public async Task<ProductVariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting variant with id {VariantId}", request.VariantId);
            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetEntityWithSpecAsync(new VariantWithDetailsSpecification(request.VariantId), cancellationToken)
                ?? throw new NotFoundException(nameof(ProductVariant), request.VariantId.ToString());

            var variantDto = mapper.Map<ProductVariantDto>(variant);
            return variantDto;
        }
    }
}
