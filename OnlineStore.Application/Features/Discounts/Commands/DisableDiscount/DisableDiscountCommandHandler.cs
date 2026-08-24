using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Commands.DisableDiscount
{
    public class DisableDiscountCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<DisableDiscountCommandHandler> logger)
    : IRequestHandler<DisableDiscountCommand, Unit>
    {
        public async Task<Unit> Handle(
            DisableDiscountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Disabling discount {DiscountId}", request.DiscountId);

            var discount = await unitOfWork.Repository<Discount>()
                .GetByIdAsync(request.DiscountId)
                ?? throw new NotFoundException(nameof(Discount), request.DiscountId.ToString());

            if (!discount.IsEnabled)
                return Unit.Value;

            discount.IsEnabled = false;
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Discount {DiscountId} disabled successfully", request.DiscountId);

            return Unit.Value;
        }
    }
}
