using MediatR;
using OnlineStore.Application.Features.ProductOptions.Dtos;

namespace OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionsByProductId
{
    public class GetProductOptionsByProductIdQuery(int productId) : IRequest<List<ProductOptionDto>>
    {
        public int ProductId { get; } = productId;
    }
}
