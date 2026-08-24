using AutoMapper;
using OnlineStore.Application.Features.Coupons.Commands.CreateCoupon;
using OnlineStore.Application.Features.Coupons.Commands.UpdateCoupon;
using OnlineStore.Application.Features.Coupons.Dtos;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Coupons.Mappings
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<CreateCouponCommand, Coupon>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code.Trim().ToUpperInvariant()));

            CreateMap<UpdateCouponCommand, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsageLimit, opt => opt.MapFrom(src => src.MaxUsageCount));
            CreateMap<Coupon, CouponDto>();
        }
    }
}
