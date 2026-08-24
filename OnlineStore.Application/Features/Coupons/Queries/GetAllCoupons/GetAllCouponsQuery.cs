using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Queries.GetAllCoupons
{
    public class GetAllCouponsQuery : IRequest<PageResult<OnlineStore.Application.Features.Coupons.Dtos.CouponDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Code { get; set; }
        public DiscountType? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
