using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.BulkCreateVariants
{
    public class BulkCreateVariantsCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<BulkCreateVariantsCommandHandler> logger)
        : IRequestHandler<BulkCreateVariantsCommand, List<int>>
    {
        public async Task<List<int>> Handle(
            BulkCreateVariantsCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Regenerating variants for product {ProductId}",
                request.ProductId);

            var product = await unitOfWork.Repository<Product>()
                .GetEntityWithSpecAsync(
                    new ProductWithOptionsAndVariantsSpecification(request.ProductId))
                ?? throw new NotFoundException(
                    nameof(Product),
                    request.ProductId.ToString());

            ValidateProduct(product);

            RemoveExistingVariants(product, request.ProductId, cancellationToken);

            var optionValueGroups = GetOptionValueGroups(product);

            var combinations = GetCartesianProduct(optionValueGroups);

            var optionValuesById = product.Options
                .SelectMany(o => o.Values)
                .ToDictionary(v => v.Id);

            var newVariants = CreateVariants(
                request.ProductId,
                combinations,
                optionValuesById);

            await unitOfWork.Repository<ProductVariant>()
                .AddRangeAsync(newVariants, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Successfully regenerated {VariantCount} variants for product {ProductId}",
                newVariants.Count,
                request.ProductId);

            return newVariants
                .Select(v => v.Id)
                .ToList();
        }

        private static void ValidateProduct(Product product)
        {
            if (!product.Options.Any())
            {
                throw new BadRequestException(
                    "Product has no options. Create options and values before generating variants.");
            }

            var emptyOptions = product.Options
                .Where(o => !o.Values.Any())
                .Select(o => o.Name)
                .ToList();

            if (emptyOptions.Count > 0)
            {
                throw new BadRequestException(
                    $"The following options have no values: " +
                    $"{string.Join(", ", emptyOptions)}. " +
                    "Create values for these options before generating variants.");
            }
        }

        private void RemoveExistingVariants(
            Product product,
            int productId,
            CancellationToken cancellationToken)
        {
            if (!product.Variants.Any())
                return;

            logger.LogWarning(
                "Removing {Count} existing variants for product {ProductId} before regeneration.",
                product.Variants.Count,
                productId);

            unitOfWork.Repository<ProductVariant>()
                .DeleteRange(product.Variants);
        }

        private static List<List<int>> GetOptionValueGroups(Product product)
        {
            return product.Options
                .OrderBy(o => o.Id)
                .Select(o => o.Values
                    .Select(v => v.Id)
                    .ToList())
                .ToList();
        }

        private static List<ProductVariant> CreateVariants(
            int productId,
            List<List<int>> combinations,
            Dictionary<int, ProductOptionValue> optionValuesById)
        {
            return combinations
                .Select((combination, index) => new ProductVariant
                {
                    ProductId = productId,
                    IsDefault = index == 0,
                    IsActive = false,

                    Options = combination
                        .Select(optionValueId =>
                        {
                            var optionValue = optionValuesById[optionValueId];

                            return new VariantOption
                            {
                                OptionValueId = optionValueId,
                                OptionId = optionValue.OptionId
                            };
                        })
                        .ToList()
                })
                .ToList();
        }

        private static List<List<int>> GetCartesianProduct(
            List<List<int>> groups)
        {
            var result = new List<List<int>>
            {
                new()
            };

            foreach (var group in groups)
            {
                result = result
                    .SelectMany(
                        combination => group.Select(
                            value => combination
                                .Append(value)
                                .ToList()))
                    .ToList();
            }

            return result;
        }
    }
}