using MediatR;

namespace OnlineStore.Application.Features.Carts.Commands.RemoveCartItem
{
    public class RemoveCartItemCommand(int productVariantId) : IRequest<Unit>
    {
        public int ProductVariantId { get; } = productVariantId;
    }
}
