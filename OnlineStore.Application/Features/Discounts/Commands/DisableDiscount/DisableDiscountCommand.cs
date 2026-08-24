using MediatR;

namespace OnlineStore.Application.Features.Discounts.Commands.DisableDiscount
{
    public class DisableDiscountCommand(int discountId) : IRequest<Unit>
    {
        public int DiscountId { get; } = discountId;
    }
}
