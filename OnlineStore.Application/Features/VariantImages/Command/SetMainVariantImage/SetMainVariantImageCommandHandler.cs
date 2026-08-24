using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.VariantImages.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Command.SetMainVariantImage
{
    public class SetMainVariantImageCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<SetMainVariantImageCommand, Unit>
    {
        public async Task<Unit> Handle(SetMainVariantImageCommand request, CancellationToken cancellationToken)
        {
            var images = await unitOfWork.Repository<ProductImage>()
                .GetAllWithSpecAsync(new ImagesByVariantIdSpecification(request.VariantId), cancellationToken);

            var targetImage = images.FirstOrDefault(i => i.Id == request.ImageId)
                ?? throw new NotFoundException(nameof(ProductImage), request.ImageId.ToString());

            if (targetImage.IsMainImage)
                return Unit.Value;

            foreach (var image in images.Where(i => i.IsMainImage))
                image.IsMainImage = false;
            targetImage.IsMainImage = true;
            await unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}