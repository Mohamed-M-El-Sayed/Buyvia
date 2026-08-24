using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Products.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler(IUnitOfWork unitOfWork
        , ILogger<DeleteProductCommandHandler> logger) : IRequestHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting product with Id: {ProductId}", request.ProductId);
            var product = await unitOfWork.Repository<Product>()
               .GetEntityWithSpecAsync(
                   new ProductWithVariantsSpecification(request.ProductId))
               ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());
            product.Delete();
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
