using MediatR;
using OnlineStore.Application.Features.Products.Dtos;

namespace OnlineStore.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery(int productId) : IRequest<ProductDetailsDto>
    {
        public int ProductId { get; } = productId;

    }
}
