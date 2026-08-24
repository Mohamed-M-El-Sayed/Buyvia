using MediatR;
using OnlineStore.Application.Features.VariantImages.Dtos;

namespace OnlineStore.Application.Features.VariantImages.Queries.GetVariantImages
{
    public class GetVariantImagesQuery(int variantId) : IRequest<List<VariantImageDto>>
    {
        public int VariantId { get; } = variantId;
    }
}
