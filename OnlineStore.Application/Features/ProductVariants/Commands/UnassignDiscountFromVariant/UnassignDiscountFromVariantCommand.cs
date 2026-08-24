using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.UnassignDiscountFromVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class UnassignDiscountFromVariantCommand(int variantId) : IRequest<Unit>
    {
        public int VariantId { get; } = variantId;
    }
}