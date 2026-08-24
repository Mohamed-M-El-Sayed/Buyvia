using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;
using OnlineStore.Application.Features.DeliveryMethods.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetAllDeliveryMethods
{
    public class GetAllDeliveryMethodsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllDeliveryMethodsQueryHandler> logger)
        : IRequestHandler<GetAllDeliveryMethodsQuery, PageResult<DeliveryMethodDto>>
    {
        public async Task<PageResult<DeliveryMethodDto>> Handle(
            GetAllDeliveryMethodsQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Getting delivery methods. PageNumber: {PageNumber}, PageSize: {PageSize}, Search: {Search}, IsActive: {IsActive}",
                request.PageNumber,
                request.PageSize,
                request.Search,
                request.IsActive);

            var dataSpecification = new DeliveryMethodsSpecification(
                request.Search,
                request.IsActive,
                request.PageNumber,
                request.PageSize,
                enablePagination: true);

            var countSpecification = new DeliveryMethodsSpecification(
                request.Search,
                request.IsActive,
                enablePagination: false);

            var deliveryMethods = await unitOfWork
                .Repository<DeliveryMethod>()
                .GetAllWithSpecAsync(dataSpecification);

            var totalCount = await unitOfWork
                .Repository<DeliveryMethod>()
                .GetCountAsync(countSpecification);

            var data = mapper.Map<List<DeliveryMethodDto>>(deliveryMethods);

            return new PageResult<DeliveryMethodDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount);
        }
    }
}