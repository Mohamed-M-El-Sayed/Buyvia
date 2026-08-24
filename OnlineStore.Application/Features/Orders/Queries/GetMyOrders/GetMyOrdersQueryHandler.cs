using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetMyOrdersQueryHandler> logger)
        : IRequestHandler<GetMyOrdersQuery, PageResult<OrderSummaryDto>>
    {
        public async Task<PageResult<OrderSummaryDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");


            logger.LogInformation(
                    "Getting orders for user {UserId}",
                    userId);

            var spec = new OrdersByUserSpecification(
                userId,
                applyPagination: true,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

            var orders = await unitOfWork
                .Repository<Order>()
                .GetAllWithSpecAsync(spec, cancellationToken);

            var totalCount = await unitOfWork
                .Repository<Order>()
                .GetCountAsync(
                    new OrdersByUserSpecification(userId),
                    cancellationToken);
            var ordersDto = mapper.Map<List<OrderSummaryDto>>(orders);

            return new PageResult<OrderSummaryDto>(ordersDto,
               request.PageNumber,
               request.PageSize,
               totalCount);


        }
    }
}
