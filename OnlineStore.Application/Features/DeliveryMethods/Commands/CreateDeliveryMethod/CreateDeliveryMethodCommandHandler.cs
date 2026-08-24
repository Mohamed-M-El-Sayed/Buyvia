using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod
{
    public class CreateDeliveryMethodCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateDeliveryMethodCommandHandler> logger,
        IMapper mapper)
        : IRequestHandler<CreateDeliveryMethodCommand, int>
    {
        public async Task<int> Handle(CreateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Creating delivery method {Name}.",
            request.Name);
            var deliveryMethod = mapper.Map<DeliveryMethod>(request);
            await unitOfWork.Repository<DeliveryMethod>().AddAsync(deliveryMethod, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation("Delivery method {Name} created with id {Id}", deliveryMethod.Name, deliveryMethod.Id);
            return deliveryMethod.Id;
        }
    }
}
