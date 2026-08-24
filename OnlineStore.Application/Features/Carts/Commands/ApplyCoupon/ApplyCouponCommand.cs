using MediatR;

namespace OnlineStore.Application.Features.Carts.Commands.ApplyCoupon
{
    public class ApplyCouponCommand : IRequest<Unit>
    {
        public string CouponCode { get; set; } = null!;
    }
}
