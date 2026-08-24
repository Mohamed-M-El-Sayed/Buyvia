using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.SetDefaultVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class SetDefaultVariantCommand(int variantId) : IRequest<Unit>
    {
        public int VariationId { get; } = variantId;
    }
}
