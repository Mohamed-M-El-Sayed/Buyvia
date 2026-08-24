using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommandHanlder(IUnitOfWork unitOfWork,
        ILogger<CreateCouponCommandHanlder> logger,
        IMapper mapper) : IRequestHandler<CreateCouponCommand, int>
    {
        public async Task<int> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
           "Creating coupon with code {CouponCode}", request.Code);
            bool codeExists = await unitOfWork.Repository<Coupon>().AnyAsync(c => c.Code == request.Code, cancellationToken);
            if (codeExists) throw new BadRequestException("Coupon code already exists.");
            request.Code = request.Code.Trim().ToUpper();
            var coupon = mapper.Map<Coupon>(request);
            await unitOfWork.Repository<Coupon>().AddAsync(coupon, cancellationToken);
            await unitOfWork.CompleteAsync();
            return coupon.Id;
        }
    }
}
