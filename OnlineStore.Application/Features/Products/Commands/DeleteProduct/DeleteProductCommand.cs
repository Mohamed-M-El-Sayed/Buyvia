using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Products.Commands.DeleteProduct
{
    [InvalidateCache(CacheTags.Products)]
    public class DeleteProductCommand(int productId) : IRequest<Unit>
    {
        public int ProductId { get; } = productId;
    }
}
