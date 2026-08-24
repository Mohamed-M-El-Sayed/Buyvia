using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Commands.CreateCoupon
{
    [InvalidateCache(CacheTags.Coupons)]
    public class CreateCouponCommand : IRequest<int>
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime? StartsAt { get; set; }
        public int? MaxUsageCount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
