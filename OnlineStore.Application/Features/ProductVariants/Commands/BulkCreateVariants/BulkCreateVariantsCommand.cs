using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.BulkCreateVariants
{
    [InvalidateCache(CacheTags.Products)]
    public class BulkCreateVariantsCommand(int productId) : IRequest<List<int>>
    {
        public int ProductId { get; } = productId;
    }
}
