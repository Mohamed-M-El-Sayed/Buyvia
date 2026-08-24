using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.DeleteDeliveryMethod
{
    public class DeleteDeliveryMethodCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteDeliveryMethodCommandHandler> logger)
        : IRequestHandler<DeleteDeliveryMethodCommand>
    {
        public async Task Handle(DeleteDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Deleting delivery method {DeliveryMethodId}.",
            request.Id);

            var entity = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(DeliveryMethod), request.Id.ToString());

            unitOfWork.Repository<DeliveryMethod>().Delete(entity);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation("Delivery method {Id} deleted", request.Id);
        }
    }
}
