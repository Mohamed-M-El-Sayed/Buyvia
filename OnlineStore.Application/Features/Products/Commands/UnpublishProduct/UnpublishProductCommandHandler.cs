using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Products.Commands.UnpublishProduct
{
    // Features/Products/Commands/UnpublishProduct/UnpublishProductCommandHandler.cs
    public class UnpublishProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UnpublishProductCommandHandler> logger)
        : IRequestHandler<UnpublishProductCommand, Unit>
    {
        public async Task<Unit> Handle(
            UnpublishProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Unpublishing product {ProductId}", request.ProductId);

            var product = await unitOfWork.Repository<Product>()
                .GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());

            // Already draft — nothing to do
            if (product.Status == ProductStatus.Draft)
                throw new BadRequestException("Product is already unpublished.");

            product.Status = ProductStatus.Draft;

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Product {ProductId} unpublished successfully", request.ProductId);

            return Unit.Value;
        }
    }
}
