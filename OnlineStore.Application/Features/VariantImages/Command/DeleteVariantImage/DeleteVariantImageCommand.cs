using MediatR;

namespace OnlineStore.Application.Features.VariantImages.Command.DeleteVariantImage
{
    public class DeleteVariantImageCommand(int variantId, int imageId) : IRequest<Unit>
    {
        public int VariantId { get; } = variantId;
        public int ImageId { get; } = imageId;

    }
}
