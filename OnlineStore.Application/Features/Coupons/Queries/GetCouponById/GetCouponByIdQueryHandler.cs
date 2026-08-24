using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Coupons.Dtos;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetCouponByIdQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetCouponByIdQuery, CouponDto>
    {
        public async Task<CouponDto> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting coupon with id: {CouponId}.", request.Id);

            var coupon = await unitOfWork.Repository<Coupon>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Coupon), request.Id.ToString());

            return mapper.Map<CouponDto>(coupon);
        }
    }
}