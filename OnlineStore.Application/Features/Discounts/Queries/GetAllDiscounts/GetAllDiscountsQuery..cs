using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Discounts.Queries.GetAllDiscounts
{
    public class GetAllDiscountsQuery : IRequest<PageResult<DiscountDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool? IsEnabled { get; set; }        // filter by enabled/disabled
        public DiscountType? Type { get; set; }     // filter by type
        public bool? IsCurrentlyActive { get; set; }     // filter by currently active
        public DiscountSortField? SortBy { get; set; } = DiscountSortField.CreatedAt;
        public bool SortDescending { get; set; } = true;

    }
}
