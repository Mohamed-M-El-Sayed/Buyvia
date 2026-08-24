using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;
using OnlineStore.Application.Features.DeliveryMethods.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetAvailableDeliveryMethods
{
    public class GetAvailableDeliveryMethodsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAvailableDeliveryMethodsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetAvailableDeliveryMethodsQuery, List<DeliveryMethodDto>>
    {
        public async Task<List<DeliveryMethodDto>> Handle(GetAvailableDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting available delivery methods");
            var deliveryMethods = await unitOfWork
             .Repository<DeliveryMethod>()
             .GetAllWithSpecAsync(new AvailableDeliveryMethodsSpecification());
            return mapper.Map<List<DeliveryMethodDto>>(deliveryMethods);
        }
    }
}
