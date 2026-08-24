using MediatR;
using OnlineStore.Application.Features.ProductOptions.Dtos;

namespace OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionById
{
    public class GetProductOptionByIdQuery(int optionId) : IRequest<ProductOptionDetailsDto>
    {
        public int OptionId { get; } = optionId;
    }
}
