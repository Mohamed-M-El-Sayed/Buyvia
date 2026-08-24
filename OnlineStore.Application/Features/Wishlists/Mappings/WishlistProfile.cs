using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.Wishlists.Dtos;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Mappings
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<Wishlist, WishlistDto>();

            CreateMap<WishlistItem, WishlistItemDto>()
             .ForMember(d => d.WishlistItemId,
                 o => o.MapFrom(s => s.Id))
             .ForMember(d => d.VariantId,
                 o => o.MapFrom(s => s.ProductVariantId))
             .ForMember(d => d.ProductId,
                o => o.MapFrom(s => s.ProductVariant.ProductId))
             .ForMember(d => d.ProductName,
                 o => o.MapFrom(s => s.ProductVariant.Product.Name))
             .ForMember(d => d.VariantName,
                 o => o.MapFrom(s => s.ProductVariant.GetVariantName()))
             .ForMember(d => d.ShortDescription,
                 o => o.MapFrom(s => s.ProductVariant.Product.ShortDescription))
             .ForMember(d => d.OriginalPrice,
                 o => o.MapFrom(s => s.ProductVariant.Price))
             .ForMember(d => d.FinalPrice,
                 o => o.MapFrom(s => s.ProductVariant.FinalPrice))
             .ForMember(d => d.HasDiscount,
                 o => o.MapFrom(s => s.ProductVariant.Discount != null))
             .ForMember(d => d.InStock,
                 o => o.MapFrom(s => s.ProductVariant.Stock > 0))
            .ForMember(
                d => d.MainImageUrl,
                o => o.ConvertUsing<ImageUrlConverter, string?>(
                    s => s.ProductVariant.Images
                        .Where(i => i.IsMainImage)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()));





        }
    }

}
