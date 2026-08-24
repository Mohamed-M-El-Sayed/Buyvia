using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Command.AddVariantImages
{
    public class AddVariantImagesCommandHandler(IUnitOfWork unitOfWork,
        ILogger<AddVariantImagesCommandHandler> logger) : IRequestHandler<AddVariantImagesCommand, Unit>
    {
        public async Task<Unit> Handle(AddVariantImagesCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Adding images to variant {VariantId}", request.VariantId);

            var variant = await unitOfWork.Repository<ProductVariant>()
              .GetEntityWithSpecAsync(new VariantWithImagesSpecification(request.VariantId));

            if (variant is null)
                throw new NotFoundException($"Product variant with ID {request.VariantId} not found");


            // If any new image is marked as main,
            // remove the main flag from all existing images.
            if (request.Images.Any(i => i.IsMainImage))
            {
                foreach (var image in variant.Images)
                {
                    image.IsMainImage = false;
                }
            }
            var nextDisplayOrder = variant.Images.Any()
                ? variant.Images.Max(v => v.DisplayOrder) + 1 : 0;

            foreach (var image in request.Images)
            {
                variant.Images.Add(new ProductImage
                {
                    ImageUrl = image.ImageUrl,
                    IsMainImage = image.IsMainImage,
                    DisplayOrder = nextDisplayOrder++
                });
            }
            unitOfWork.Repository<ProductVariant>().Update(variant);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

