using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Commands.SetCouponActiveStatus
{
    public class SetCouponActiveStatusCommandHandler(IUnitOfWork unitOfWork,
        ILogger<SetCouponActiveStatusCommandHandler> logger) : IRequestHandler<SetCouponActiveStatusCommand, Unit>
    {
        public async Task<Unit> Handle(SetCouponActiveStatusCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Setting active status for coupon {CouponId} to {IsActive}", request.CouponId, request.IsActive);

            var coupon = await unitOfWork.Repository<Coupon>().GetByIdAsync(request.CouponId)
                ?? throw new NotFoundException(nameof(Coupon), request.CouponId.ToString());

            if (coupon.IsActive == request.IsActive)
                return Unit.Value;

            coupon.IsActive = request.IsActive;

            unitOfWork.Repository<Coupon>().Update(coupon);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation("Coupon {CouponId} status set to {IsActive}", request.CouponId, request.IsActive);

            return Unit.Value;
        }
    }
}
