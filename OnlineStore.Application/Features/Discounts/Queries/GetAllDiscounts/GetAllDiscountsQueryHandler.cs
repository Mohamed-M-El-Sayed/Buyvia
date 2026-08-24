using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Application.Features.Discounts.Specifications;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Queries.GetAllDiscounts
{
    public class GetAllDiscountsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllDiscountsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetAllDiscountsQuery, PageResult<DiscountDto>>
    {
        public async Task<PageResult<DiscountDto>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Getting discounts. Page: {PageNumber}, PageSize: {PageSize}",
                request.PageNumber,
                request.PageSize);
            var countSpec = new DiscountsSpecification(
               request,
               applyPaging: false, applySorting: false);
            var totalCount = await unitOfWork.Repository<Discount>()
               .GetCountAsync(countSpec, cancellationToken);
            var dataSpec = new DiscountsSpecification(request);
            var discounts = await unitOfWork.Repository<Discount>()
                 .GetAllWithSpecAsync(dataSpec, cancellationToken);

            return new PageResult<DiscountDto>(
                 mapper.Map<List<DiscountDto>>(discounts),
                 request.PageNumber,
                 request.PageSize,
                 totalCount
            );

        }
    }
}
