using MediatR;

namespace OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOption
{
    public class DeleteProductOptionCommand(int optionId) : IRequest
    {
        public int OptionId { get; } = optionId;
    }
}
