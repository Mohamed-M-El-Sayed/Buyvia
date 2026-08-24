using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Application.Features.VariantImages.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Queries.GetVariantImages
{
    public class GetVariantImagesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetVariantImagesQueryHandler> logger,
        IMapper mapper)
        : IRequestHandler<GetVariantImagesQuery, List<VariantImageDto>>
    {
        public async Task<List<VariantImageDto>> Handle(GetVariantImagesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                      "Retrieving images for variant {VariantId}.", request.VariantId);
            var variant = await unitOfWork.Repository<ProductVariant>()
           .GetEntityWithSpecAsync(
               new VariantWithImagesSpecification(request.VariantId))
                   ?? throw new NotFoundException(
                       nameof(ProductVariant),
                       request.VariantId.ToString());
            return mapper.Map<List<VariantImageDto>>(variant.Images);
        }
    }
}
