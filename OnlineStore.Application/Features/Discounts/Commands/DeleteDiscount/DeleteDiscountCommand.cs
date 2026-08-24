using MediatR;

namespace OnlineStore.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommand(int discountId) : IRequest<Unit>
    {
        public int DiscountId { get; } = discountId;
    }
}
