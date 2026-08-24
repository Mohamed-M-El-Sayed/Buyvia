using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.BulkUpdateVariants
{
    public class BulkUpdateVariantsCommandHandler(IUnitOfWork unitOfWork,
        ILogger<BulkUpdateVariantsCommandHandler> logger) : IRequestHandler<BulkUpdateVariantsCommand, Unit>
    {
        public async Task<Unit> Handle(BulkUpdateVariantsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Bulk updating {Count} variants for product {ProductId}",
            request.Variants.Count, request.ProductId);

            var product = await unitOfWork.Repository<Product>()
                .GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());
            var variantIds = request.Variants.Select(v => v.Id).ToList();


            var variants = await unitOfWork.Repository<ProductVariant>()
                .GetAllWithSpecAsync(new VariantsByProductIdSpecification(request.ProductId, variantIds));

            var foundIds = variants.Select(v => v.Id).ToHashSet();
            var invalidIds = variantIds.Where(id => !foundIds.Contains(id)).ToList();
            if (invalidIds.Any())
                throw new BadRequestException(
                                $"The following variant IDs do not belong to product {request.ProductId}: {string.Join(", ", invalidIds)}");

            foreach (var dto in request.Variants)
            {
                var variant = variants.First(v => v.Id == dto.Id);
                variant.Price = dto.Price;
                variant.Stock = dto.Stock;
                variant.StockThreshold = dto.StockThreshold;
                variant.IsActive = dto.IsActive;
            }
            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
