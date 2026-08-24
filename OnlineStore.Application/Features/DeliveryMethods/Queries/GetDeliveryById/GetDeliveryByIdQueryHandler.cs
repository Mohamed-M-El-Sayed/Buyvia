using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDeliveryByIdQuery, OrderDeliveryMethodDto>
    {
        public async Task<OrderDeliveryMethodDto> Handle(GetDeliveryByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(DeliveryMethod), request.Id.ToString());

            return mapper.Map<OrderDeliveryMethodDto>(entity);
        }
    }
}
