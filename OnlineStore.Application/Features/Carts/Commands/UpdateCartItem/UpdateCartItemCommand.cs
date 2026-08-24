using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
