using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Commands.EnableDiscount
{
    public class EnableDiscountCommandHandler(IUnitOfWork unitOfWork,
        ILogger<EnableDiscountCommandHandler> logger) : IRequestHandler<EnableDiscountCommand, Unit>
    {
        public async Task<Unit> Handle(EnableDiscountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Enabling discount {DiscountId}", request.DiscountId);

            var discount = await unitOfWork.Repository<Discount>()
                .GetByIdAsync(request.DiscountId)
                ?? throw new NotFoundException(nameof(Discount), request.DiscountId.ToString());

            if (discount.IsEnabled)
                return Unit.Value;

            discount.IsEnabled = true;
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation(
          "Discount {DiscountId} enabled successfully", request.DiscountId);
            return Unit.Value;
        }
    }
}
