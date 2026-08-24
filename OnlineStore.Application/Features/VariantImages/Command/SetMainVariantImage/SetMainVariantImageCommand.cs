using MediatR;

namespace OnlineStore.Application.Features.VariantImages.Command.SetMainVariantImage
{
    public class SetMainVariantImageCommand(int variantId, int imageId) : IRequest<Unit>
    {
        public int VariantId { get; } = variantId;
        public int ImageId { get; } = imageId;
    }

}
