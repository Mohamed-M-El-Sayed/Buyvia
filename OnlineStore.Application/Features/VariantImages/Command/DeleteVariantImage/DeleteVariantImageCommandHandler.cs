using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Command.DeleteVariantImage
{
    public class DeleteVariantImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        ILogger<DeleteVariantImageCommandHandler> logger)
        : IRequestHandler<DeleteVariantImageCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteVariantImageCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Deleting image {ImageId} from variant {VariantId}",
                request.ImageId, request.VariantId);

            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetEntityWithSpecAsync(
                    new VariantWithImagesSpecification(request.VariantId))
                ?? throw new NotFoundException(nameof(ProductVariant), request.VariantId.ToString());

            var image = variant.Images.FirstOrDefault(i => i.Id == request.ImageId)
                ?? throw new NotFoundException(nameof(ProductImage), request.ImageId.ToString());

            // Block deleting the last image
            if (variant.Images.Count == 1)
                throw new BadRequestException(
                    "Cannot delete the last image. A variant must have at least one image.");

            // Block deleting the main image while other images exist
            if (image.IsMainImage)
                throw new BadRequestException("Cannot delete the main image while other images exist. Set another image as main first.");
            fileService.Delete(image.ImageUrl);
            unitOfWork.Repository<ProductImage>().Delete(image);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Image {ImageId} deleted from variant {VariantId}",
                request.ImageId, request.VariantId);

            return Unit.Value;
        }
    }
}
