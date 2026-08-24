using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommandHandler(IUnitOfWork unitOfWork
        , ILogger<DeleteBrandCommandHandler> logger) : IRequestHandler<DeleteBrandCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting brand {BrandId}", request.BrandId);
            var brand = await unitOfWork.Repository<ProductBrand>()
                .GetByIdAsync(request.BrandId)
                ?? throw new NotFoundException(nameof(ProductBrand), request.BrandId.ToString());

            // block if brand is linked to any products 
            var hasProducts = await unitOfWork.Repository<Product>()
            .AnyAsync(p => p.BrandId == request.BrandId, cancellationToken);
            if (hasProducts)
                throw new BadRequestException(
                    "Cannot delete this brand because it has products " +
                    "assigned to it. Reassign or delete the products first.");

            unitOfWork.Repository<ProductBrand>().Delete(brand);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Brand {BrandId} deleted successfully", request.BrandId);
            return Unit.Value;
        }
    }
}
