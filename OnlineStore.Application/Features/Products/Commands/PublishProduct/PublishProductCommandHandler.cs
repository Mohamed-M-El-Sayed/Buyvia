using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Products.Specifications;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Products.Commands.PublishProduct
{
    public class PublishProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<PublishProductCommandHandler> logger)
        : IRequestHandler<PublishProductCommand, Unit>
    {
        public async Task<Unit> Handle(PublishProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Publishing product {ProductId}", request.ProductId);
            var product = await unitOfWork.Repository<Product>()
            .GetEntityWithSpecAsync(
                new ProductForPublishSpecification(request.ProductId))
            ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());

            if (product.Status == ProductStatus.Published)
                throw new BadRequestException("Product is already published.");
            var errors = new List<string>();

            if (product.CategoryId is null)
                errors.Add("Product must have a category.");

            if (product.BrandId is null)
                errors.Add("Product must have a brand.");

            var activeVariants = product.Variants
            .Where(v => v.IsActive)
            .ToList();

            if (!activeVariants.Any())
                errors.Add("Product must have at least one active variant.");
            var unpricedVariants = activeVariants
            .Where(v => v.Price <= 0)
            .ToList();

            if (unpricedVariants.Any())
                errors.Add($"{unpricedVariants.Count} variant(s) have no price set.");

            var variantsWithoutImages = activeVariants
            .Where(v => !v.Images.Any())
            .ToList();

            if (variantsWithoutImages.Any())
                errors.Add($"{variantsWithoutImages.Count} variant(s) have no images.");
            if (errors.Any())
                throw new BadRequestException(string.Join(" ", errors));
            product.Status = ProductStatus.Published;
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Product {ProductId} published successfully", request.ProductId);
            return Unit.Value;

        }
    }
}
