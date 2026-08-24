using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.DeleteVariant
{

    public class DeleteVariantCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteVariantCommandHandler> logger) : IRequestHandler<DeleteVariantCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteVariantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Deleting variant {VariantId}", request.VariantId);

            // 1. Load variant with siblings
            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetEntityWithSpecAsync(
                    new VariantWithSiblingsSpecification(request.VariantId))
                ?? throw new NotFoundException(nameof(ProductVariant), request.VariantId.ToString());

            var siblings = variant.Product.Variants.ToList();

            // 1 Block deleting the last variant
            if (siblings.Count == 1)
                throw new BadRequestException(
                    "Cannot delete the last variant. Delete the product instead.");


            // If deleting the default variant — promote another one
            if (variant.IsDefault)
            {
                var nextDefault = siblings
                    .Where(v => v.Id != variant.Id)
                    .OrderByDescending(v => v.Stock)
                    .First();

                nextDefault.IsDefault = true;
            }

            variant.Delete();

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Variant {VariantId} deleted from product {ProductId}",
                request.VariantId, variant.ProductId);

            return Unit.Value;
        }
    }

}
