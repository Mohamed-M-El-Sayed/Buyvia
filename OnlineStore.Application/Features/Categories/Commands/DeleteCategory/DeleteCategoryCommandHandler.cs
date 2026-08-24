using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Categories.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger)
        : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Deleting category {CategoryId}", request.Id);

            var category = await unitOfWork.Repository<ProductCategory>()
            .GetEntityWithSpecAsync(
                new CategoryWithSubCategoriesSpecification(request.Id))
            ?? throw new NotFoundException(
                nameof(ProductCategory), request.Id.ToString());

            // Block if has subcategories
            if (category.SubCategories.Any())
            {
                throw new BadRequestException(
                    $"Cannot delete category '{category.Name}' because it has subcategories.");
            }
            //  Block if has products
            var hasProducts = await unitOfWork.Repository<Product>()
            .AnyAsync(p => p.CategoryId == request.Id, cancellationToken);

            if (hasProducts)
                throw new BadRequestException(
                    $"Cannot delete '{category.Name}' because it has products assigned to it. Reassign or delete the products first.");

            unitOfWork.Repository<ProductCategory>().Delete(category);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Category {CategoryId} deleted successfully", request.Id);

            return Unit.Value;

        }
    }
}
