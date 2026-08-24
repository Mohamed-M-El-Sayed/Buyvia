using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.ProductVariants.Commands.AssignDiscountToVariant
{
    public class AssignDiscountToVariantCommandHandler(IUnitOfWork unitOfWork,
        ILogger<AssignDiscountToVariantCommandHandler> logger) : IRequestHandler<AssignDiscountToVariantCommand>
    {
        public async Task Handle(AssignDiscountToVariantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning Discount {DiscountId} to ProductVariant {VariantId}",
                request.DiscountId, request.VariantId);
            var variant = await unitOfWork.Repository<ProductVariant>().GetByIdAsync(request.VariantId)
                ?? throw new NotFoundException(nameof(ProductVariant), request.VariantId.ToString());
            var discount = await unitOfWork.Repository<Discount>().GetByIdAsync(request.DiscountId)
                 ?? throw new NotFoundException(nameof(Discount), request.DiscountId.ToString());
            if (!discount.IsActive())
                throw new BadRequestException("Discount is not currently active.");
            variant.DiscountId = request.DiscountId;
            unitOfWork.Repository<ProductVariant>().Update(variant);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation(
                "Discount {DiscountId} assigned to ProductVariant {VariantId}",
                request.DiscountId,
                request.VariantId);
        }
    }
}
