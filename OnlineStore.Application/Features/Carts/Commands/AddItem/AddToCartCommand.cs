using MediatR;

namespace OnlineStore.Application.Features.Carts.Commands.AddItem
{
    public class AddToCartCommand : IRequest<Unit>
    {
        public int VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
