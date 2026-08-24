using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Coupons.Dtos;
using OnlineStore.Application.Features.Coupons.Specifications;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Queries.GetAllCoupons
{
    public class GetAllCouponsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllCouponsQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAllCouponsQuery, PageResult<CouponDto>>
    {
        public async Task<PageResult<CouponDto>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Getting coupons. Page: {PageNumber}, Size: {PageSize}, Search: {Search}, Type: {Type}, IsActive: {IsActive}",
            request.PageNumber,
            request.PageSize,
            request.Code,
            request.Type,
            request.IsActive);

            var specification = new CouponsSpecification(
                request.Code,
                request.Type,
                request.IsActive,
                request.PageNumber,
                request.PageSize);

            var coupons = await unitOfWork
                .Repository<Coupon>()
                .GetAllWithSpecAsync(specification);

            var countSpecification = new CouponsCountSpecification(
                request.Code,
                request.Type,
                request.IsActive);

            var totalCount = await unitOfWork
                .Repository<Coupon>()
                .GetCountAsync(countSpecification);

            var couponDtos = mapper.Map<List<CouponDto>>(coupons);

            return new PageResult<CouponDto>(couponDtos, request.PageNumber, request.PageSize, totalCount);

        }
    }
}
