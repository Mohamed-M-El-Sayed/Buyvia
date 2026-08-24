using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Coupons.Commands.DeleteCoupon
{
    [InvalidateCache(CacheTags.Coupons)]
    public class DeleteCouponCommand(int id) : IRequest
    {
        public int Id { get; } = id;
    }
}
