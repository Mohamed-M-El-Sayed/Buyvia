using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOption
{
    public class UpdateProductOptionCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateProductOptionCommand>
    {
        public async Task Handle(UpdateProductOptionCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException("Option name is required.");

            var option = await unitOfWork.Repository<ProductOption>()
                .GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ProductOption), request.Id.ToString());

            var duplicateOption = await unitOfWork.Repository<ProductOption>()
                .AnyAsync(o => o.ProductId == option.ProductId && o.Id != option.Id && o.Name == request.Name, cancellationToken);

            if (duplicateOption)
                throw new BadRequestException("A product option with the same name already exists for this product.");

            option.Name = request.Name.Trim();

            unitOfWork.Repository<ProductOption>().Update(option);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
