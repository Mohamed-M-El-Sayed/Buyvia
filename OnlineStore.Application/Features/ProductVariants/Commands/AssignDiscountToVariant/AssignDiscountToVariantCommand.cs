using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.AssignDiscountToVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class AssignDiscountToVariantCommand(int variantId, int discountId) : IRequest
    {
        public int VariantId { get; } = variantId;
        public int DiscountId { get; } = discountId;
    }
}
