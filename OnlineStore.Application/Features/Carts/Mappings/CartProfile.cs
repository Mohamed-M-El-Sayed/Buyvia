using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.Carts.Dtos;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CouponCode,
                    opt => opt.MapFrom(src => src.Coupon != null ? src.Coupon.Code : null));
            CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.VariantName,
                opt => opt.MapFrom(src => src.ProductVariant.GetVariantName()))
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductVariant.Product.Name))
           .ForMember(dest => dest.DiscountAmount,
                opt => opt.MapFrom(src =>
                    src.ProductVariant.Discount != null
                    ? src.ProductVariant.Discount.CalculateDiscount(src.UnitPrice)
                    : 0m))
           .ForMember(dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductVariant.ProductId))
           .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductVariant.Product.Name))
           .ForMember(dest => dest.ImageUrl,
                opt => opt.ConvertUsing<ImageUrlConverter, string?>(
                    src => src.ProductVariant.Images.FirstOrDefault(i => i.IsMainImage) != null
                    ? src.ProductVariant.Images.FirstOrDefault(i => i.IsMainImage)!.ImageUrl
                    : null));



        }
    }
}
