using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.UnassignDiscountFromVariant
{
    public class UnassignDiscountFromVariantCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UnassignDiscountFromVariantCommandHandler> logger)
        : IRequestHandler<UnassignDiscountFromVariantCommand, Unit>
    {
        public async Task<Unit> Handle(
            UnassignDiscountFromVariantCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Unassigning Discount from ProductVariant {VariantId}",
                request.VariantId);

            var variant = await unitOfWork
                .Repository<ProductVariant>()
                .GetByIdAsync(request.VariantId)
                ?? throw new NotFoundException(
                    nameof(ProductVariant),
                    request.VariantId.ToString());

            if (variant.DiscountId is null)
                throw new BadRequestException(
                    "Product variant does not have a discount assigned.");

            variant.DiscountId = null;

            unitOfWork.Repository<ProductVariant>().Update(variant);

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Discount unassigned from ProductVariant {VariantId}",
                request.VariantId);
            return Unit.Value;
        }
    }
}