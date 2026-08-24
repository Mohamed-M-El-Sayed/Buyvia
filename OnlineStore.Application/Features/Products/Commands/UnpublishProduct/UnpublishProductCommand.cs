using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Products.Commands.UnpublishProduct
{
    [InvalidateCache(CacheTags.Products)]
    public class UnpublishProductCommand(int productId) : IRequest<Unit>
    {
        public int ProductId { get; set; } = productId;

    }
}
