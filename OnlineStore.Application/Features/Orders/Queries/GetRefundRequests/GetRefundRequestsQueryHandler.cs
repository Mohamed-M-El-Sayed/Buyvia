using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Queries.GetRefundRequests
{
    public class GetRefundRequestsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<GetRefundRequestsQuery, PageResult<RefundRequestDto>>
    {
        public async Task<PageResult<RefundRequestDto>> Handle(
            GetRefundRequestsQuery request,
            CancellationToken cancellationToken)
        {
            // Count without pagination
            var countSpec = new RefundRequestsSpecification(
                request.Status,
                isPagingEnabled: false);

            var totalCount = await unitOfWork
                .Repository<RefundRequest>()
                .GetCountAsync(countSpec);

            // Get paginated data
            var dataSpec = new RefundRequestsSpecification(
                request.Status,
                request.PageNumber,
                request.PageSize);

            var refundRequests = await unitOfWork
                .Repository<RefundRequest>()
                .GetAllWithSpecAsync(dataSpec);

            var data = mapper.Map<List<RefundRequestDto>>(refundRequests);

            return new PageResult<RefundRequestDto>(data, request.PageNumber, request.PageSize, totalCount);

        }
    }
}
