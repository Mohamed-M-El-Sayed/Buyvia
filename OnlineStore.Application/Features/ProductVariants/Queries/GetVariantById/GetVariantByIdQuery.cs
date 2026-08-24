using MediatR;
using OnlineStore.Application.Features.ProductVariants.Dtos;

namespace OnlineStore.Application.Features.ProductVariants.Queries.GetVariantById
{
    public class GetVariantByIdQuery(int variantId) : IRequest<ProductVariantDto>
    {
        public int VariantId { get; } = variantId;
    }
}
