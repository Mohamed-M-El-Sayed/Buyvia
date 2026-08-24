using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOption
{
    public class CreateProductOptionCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateProductOptionCommand, CreateProductOptionResponseDto>
    {
        public async Task<CreateProductOptionResponseDto> Handle(
            CreateProductOptionCommand request,
            CancellationToken cancellationToken)
        {
            var productExists = await unitOfWork.Repository<Product>()
                .AnyAsync(
                    p => p.Id == request.ProductId,
                    cancellationToken);

            if (!productExists)
                throw new NotFoundException(
                    nameof(Product),
                    request.ProductId.ToString());

            var optionName = request.Name.Trim();

            var duplicateOption = await unitOfWork.Repository<ProductOption>()
                .AnyAsync(
                    o => o.ProductId == request.ProductId &&
                         o.Name == optionName,
                    cancellationToken);

            if (duplicateOption)
                throw new BadRequestException(
                    "A product option with the same name already exists for this product.");

            var hasExistingVariants = await unitOfWork.Repository<ProductVariant>()
                .AnyAsync(
                    v => v.ProductId == request.ProductId,
                    cancellationToken);

            var option = new ProductOption
            {
                ProductId = request.ProductId,
                Name = optionName
            };

            foreach (var value in request.Values)
            {
                option.Values.Add(new ProductOptionValue
                {
                    Value = value.Value.Trim()
                });
            }

            await unitOfWork.Repository<ProductOption>()
                .AddAsync(option, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            return new CreateProductOptionResponseDto
            {
                OptionId = option.Id,
                HasExistingVariants = hasExistingVariants,
                Warning = hasExistingVariants
                    ? "This product already has variants. Generate the missing variants to include the new option."
                    : null
            };
        }
    }
}