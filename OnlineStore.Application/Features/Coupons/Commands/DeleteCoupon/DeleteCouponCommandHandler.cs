using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Commands.DeleteCoupon
{
    public class DeleteCouponCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteCouponCommandHandler> logger) : IRequestHandler<DeleteCouponCommand>
    {
        public async Task Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting coupon with id: {CouponId}.", request.Id);

            var coupon = await unitOfWork.Repository<Coupon>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Coupon), request.Id.ToString());
            coupon.Delete();
            coupon.IsActive = false;
            unitOfWork.Repository<Coupon>().Update(coupon);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
