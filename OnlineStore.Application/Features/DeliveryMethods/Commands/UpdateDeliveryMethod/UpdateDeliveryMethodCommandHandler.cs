using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.UpdateDeliveryMethod
{
    public class UpdateDeliveryMethodCommandHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateDeliveryMethodCommandHandler> logger,
        IMapper mapper)
        : IRequestHandler<UpdateDeliveryMethodCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Updating delivery method {DeliveryMethodId}.",
            request.Id);

            var entity = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(DeliveryMethod), request.Id.ToString());
            mapper.Map(request, entity);
            unitOfWork.Repository<DeliveryMethod>().Update(entity);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
