using MediatR;
using OnlineStore.Application.Features.ProductVariants.Dtos;

namespace OnlineStore.Application.Features.ProductVariants.Queries.GetVariantsByProductId
{
    public class GetVariantsByProductIdQuery(int productId) : IRequest<AdminProductVariantsDto>
    {
        public int ProductId { get; } = productId;
    }
}
