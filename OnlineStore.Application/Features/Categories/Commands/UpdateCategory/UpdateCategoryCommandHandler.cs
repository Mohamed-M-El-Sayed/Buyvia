using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateCategoryCommandHandler> logger)
        : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating category {CategoryId}", request.Id);

            var category = await unitOfWork.Repository<ProductCategory>()
                .GetByIdAsync(request.Id)
                 ?? throw new NotFoundException(nameof(ProductCategory), request.Id.ToString());
            category.Name = request.Name;
            category.Description = request.Description;
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation(
                "Category {CategoryId} updated successfully", request.Id);
            return Unit.Value;


        }
    }
}
