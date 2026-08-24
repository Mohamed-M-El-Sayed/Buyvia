using AutoMapper;
using OnlineStore.Application.Features.Discounts.Commands.CreateDiscount;
using OnlineStore.Application.Features.Discounts.Commands.UpdateDiscount;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Mappings
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<CreateDiscountCommand, Discount>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
            CreateMap<UpdateDiscountCommand, Discount>();
            CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.IsCurrentlyActive,
                opt => opt.MapFrom(src => src.IsActive()));

        }
    }
}
