using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommandHandler(IMapper mapper,
        ILogger<CreateDiscountCommandHandler> logger,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateDiscountCommand, int>
    {
        public async Task<int> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating discount {Name}", request.Name);

            var normalizedName = request.Name.Trim().ToLower();

            var exists = await unitOfWork.Repository<Discount>()
                .AnyAsync(d => d.Name.ToLower() == normalizedName, cancellationToken);
            if (exists)
                throw new BadRequestException($"Discount with name '{request.Name}' already exists.");


            var discount = mapper.Map<Discount>(request);
            await unitOfWork.Repository<Discount>()
                .AddAsync(discount, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            return discount.Id;
        }
    }
}
