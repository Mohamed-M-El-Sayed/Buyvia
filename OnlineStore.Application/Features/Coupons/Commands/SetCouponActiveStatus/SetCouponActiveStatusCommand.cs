using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Coupons.Commands.SetCouponActiveStatus
{
    [InvalidateCache(CacheTags.Coupons)]
    public class SetCouponActiveStatusCommand(int couponId, bool isActive) : IRequest<Unit>
    {
        public int CouponId { get; } = couponId;
        public bool IsActive { get; } = isActive;
    }
}
