using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Commands.UpdateCoupon
{
    public class UpdateCouponCommandHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateCouponCommandHandler> logger,
        IMapper mapper) : IRequestHandler<UpdateCouponCommand>
    {
        public async Task Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating coupon with id: {CouponId}.", request.Id);
            var coupon = await unitOfWork.Repository<Coupon>().GetByIdAsync(request.Id)
                 ?? throw new NotFoundException(nameof(Coupon), request.Id.ToString());
            coupon = mapper.Map(request, coupon);
            unitOfWork.Repository<Coupon>().Update(coupon);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
