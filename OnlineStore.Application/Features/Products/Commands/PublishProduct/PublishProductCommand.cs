using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Products.Commands.PublishProduct
{
    [InvalidateCache(CacheTags.Products)]
    public class PublishProductCommand(int productId) : IRequest<Unit>
    {
        public int ProductId { get; set; } = productId;
    }
}
