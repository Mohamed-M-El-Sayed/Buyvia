using MediatR;
using OnlineStore.Application.Features.Products.Dtos;

namespace OnlineStore.Application.Features.Products.Queries.GetProductForEdit
{
    public class GetProductForEditQuery(int productId) : IRequest<ProductEditDto>
    {
        public int ProductId { get; } = productId;
    }
}

