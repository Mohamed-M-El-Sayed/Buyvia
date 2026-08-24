using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.SetDeliveryMethodActiveStatus
{
    public class SetDeliveryMethodActiveStatusCommandHandler(IUnitOfWork unitOfWork,
        ILogger<SetDeliveryMethodActiveStatusCommandHandler> logger) : IRequestHandler<SetDeliveryMethodActiveStatusCommand, Unit>
    {
        public async Task<Unit> Handle(SetDeliveryMethodActiveStatusCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Setting active status for delivery method {DeliveryMethodId} to {IsActive}", request.DeliveryMethodId, request.IsActive);

            var entity = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(request.DeliveryMethodId)
                ?? throw new NotFoundException(nameof(DeliveryMethod), request.DeliveryMethodId.ToString());

            if (entity.IsActive == request.IsActive)
                return Unit.Value;

            entity.IsActive = request.IsActive;

            unitOfWork.Repository<DeliveryMethod>().Update(entity);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation("Delivery method {DeliveryMethodId} status set to {IsActive}", request.DeliveryMethodId, request.IsActive);

            return Unit.Value;
        }
    }
}
