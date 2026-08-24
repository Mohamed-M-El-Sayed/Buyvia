using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<UpdateDiscountCommandHandler> logger) : IRequestHandler<UpdateDiscountCommand, DiscountDto>
    {
        public async Task<DiscountDto> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Updating discount {DiscountId} with Type: {Type}, Value: {Value}",
                    request.Id, request.Type, request.Value);
            var discount = await unitOfWork.Repository<Discount>().GetByIdAsync(request.Id, tracking: false)
                ?? throw new NotFoundException(nameof(Discount), request.Id.ToString());
            var result = mapper.Map<Discount>(request);
            mapper.Map(request, discount);
            unitOfWork.Repository<Discount>().Update(result);
            await unitOfWork.CompleteAsync(cancellationToken);
            return mapper.Map<DiscountDto>(discount);
        }
    }
}
