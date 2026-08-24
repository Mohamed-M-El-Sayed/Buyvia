using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetAllDeliveryMethods
{
    public class GetAllDeliveryMethodsQuery : IRequest<PageResult<DeliveryMethodDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; } = null;
        public bool? IsActive { get; set; } = null;
    }
}
