using AutoMapper;
using OnlineStore.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod;
using OnlineStore.Application.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Mappings
{
    public class DeliveryMethodsProfile : Profile
    {
        public DeliveryMethodsProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodDto>();
            CreateMap<CreateDeliveryMethodCommand, DeliveryMethod>();
            CreateMap<DeliveryMethod, CreateDeliveryMethodCommand>();
            CreateMap<UpdateDeliveryMethodCommand, DeliveryMethod>();
        }
    }
}
