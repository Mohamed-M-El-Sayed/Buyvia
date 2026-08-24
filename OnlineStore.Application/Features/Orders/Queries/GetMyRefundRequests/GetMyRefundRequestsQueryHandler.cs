using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Queries.GetMyRefundRequests
{
    public class GetMyRefundRequestsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
        : IRequestHandler<GetMyRefundRequestsQuery, PageResult<RefundRequestDto>>
    {
        public async Task<PageResult<RefundRequestDto>> Handle(
            GetMyRefundRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var countSpec = new MyRefundRequestsSpecification(
                userId,
                request.Status,
                isPagingEnabled: false);

            var totalCount = await unitOfWork
                .Repository<RefundRequest>()
                .GetCountAsync(countSpec);

            var dataSpec = new MyRefundRequestsSpecification(
                userId,
                request.Status,
                request.PageNumber,
                request.PageSize);

            var refundRequests = await unitOfWork
                .Repository<RefundRequest>()
                .GetAllWithSpecAsync(dataSpec);

            var data = mapper.Map<List<RefundRequestDto>>(refundRequests);

            return new PageResult<RefundRequestDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount);
        }
    }
}