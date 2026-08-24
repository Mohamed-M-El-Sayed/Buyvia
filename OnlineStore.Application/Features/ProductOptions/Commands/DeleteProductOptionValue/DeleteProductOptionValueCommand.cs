using MediatR;

namespace OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOptionValue
{
    public class DeleteProductOptionValueCommand(int valueId) : IRequest
    {
        public int ValueId { get; } = valueId;
    }
}
