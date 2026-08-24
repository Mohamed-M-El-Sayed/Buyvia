using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateCategoryCommandHandler> logger)
        : IRequestHandler<CreateCategoryCommand, int>
    {
        private const int MaxDepth = 3;

        public async Task<int> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Creating category {Name} under ParentId {ParentId}",
                request.Name,
                request.ParentId);

            // Root category
            if (request.ParentId is null)
            {
                return await CreateAndSave(request, cancellationToken);
            }

            var parentDepth = await unitOfWork.Categories
                .GetDepthAsync(
                    request.ParentId.Value,
                    cancellationToken);

            if (parentDepth >= MaxDepth)
            {
                throw new BadRequestException(
                    $"Cannot add a subcategory here. " +
                    $"Maximum category depth is {MaxDepth} levels.");
            }

            return await CreateAndSave(request, cancellationToken);
        }

        private async Task<int> CreateAndSave(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = new ProductCategory
            {
                Name = request.Name,
                Description = request.Description,
                ParentId = request.ParentId
            };

            await unitOfWork.Repository<ProductCategory>()
                .AddAsync(category, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Category {Name} created with Id {Id}",
                category.Name,
                category.Id);

            return category.Id;
        }
    }
}