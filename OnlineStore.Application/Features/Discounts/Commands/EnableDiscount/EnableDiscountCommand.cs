using MediatR;

namespace OnlineStore.Application.Features.Discounts.Commands.EnableDiscount
{
    public class EnableDiscountCommand(int discountId) : IRequest<Unit>
    {
        public int DiscountId { get; } = discountId;
    }
}
