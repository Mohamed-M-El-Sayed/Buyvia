using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Mapping
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod));
            CreateMap<OrderAddress, OrderAddressDto>();
            CreateMap<OrderAddressDto, OrderAddress>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(
                d => d.ImageUrl,
                o => o.ConvertUsing<ImageUrlConverter, string?>(
                    s => s.ImageUrl));

            CreateMap<OrderAddress, OrderAddressDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<DeliveryMethod, OrderDeliveryMethodDto>();

            CreateMap<RefundRequest, RefundRequestDto>();
            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)));
            CreateMap<OrderItem, OrderSummaryItemDto>()
               .ForMember(
                d => d.ImageUrl,
                o => o.ConvertUsing<ImageUrlConverter, string?>(
                    s => s.ImageUrl));
        }
    }
}
