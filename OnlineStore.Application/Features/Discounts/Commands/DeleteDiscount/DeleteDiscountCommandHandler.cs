using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteDiscountCommandHandler> logger) : IRequestHandler<DeleteDiscountCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
          "Deleting discount {DiscountId}", request.DiscountId);
            var discount = await unitOfWork.Repository<Discount>()
            .GetByIdAsync
                 (request.DiscountId)
            ?? throw new NotFoundException(nameof(Discount), request.DiscountId.ToString());

            // Block if active and assigned to variants
            if (discount.IsActive() && discount.Variants.Any())
                throw new BadRequestException(
                     $"Cannot delete an active discount that is assigned to {discount.Variants.Count} variant(s). Disable it and remove it from variants first.");
            // If inactive but still assigned
            if (discount.Variants.Any())
                throw new BadRequestException(
                    $"Cannot delete a discount that is assigned to {discount.Variants.Count} variant(s). Remove it from variants first.");
            discount.Delete();
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Discount {DiscountId} soft-deleted successfully", request.DiscountId);
            return Unit.Value;
        }
    }
}
