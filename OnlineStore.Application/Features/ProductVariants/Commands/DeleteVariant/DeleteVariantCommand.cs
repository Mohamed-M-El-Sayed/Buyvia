using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.DeleteVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class DeleteVariantCommand(int variantId) : IRequest<Unit>
    {
        public int VariantId { get; } = variantId;
    }
}
