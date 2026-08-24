using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.UpdateVariant
{
    public class UpdateVariantCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateVariantCommandHandler> logger) : IRequestHandler<UpdateVariantCommand>
    {
        public async Task Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating variant {VariantId}", request.Id);

            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ProductVariant), request.Id.ToString());

            variant.Price = request.Price;
            variant.Stock = request.Stock;
            variant.StockThreshold = request.StockThreshold;
            variant.IsActive = request.IsActive;

            unitOfWork.Repository<ProductVariant>().Update(variant);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}