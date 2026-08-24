using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetOrderByIdQueryHandler> logger,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");

            logger.LogInformation(
                            "Getting order {OrderId} for user {UserId}",
                            request.OrderId,
                            userId);
            var order = await unitOfWork.Repository<Order>()
                 .GetEntityWithSpecAsync(new OrderByIdSpecification(request.OrderId, userId))
                 ?? throw new NotFoundException(nameof(Order), request.OrderId.ToString());
            return mapper.Map<OrderDto>(order);
        }
    }
}
