using AutoMapper;
using OnlineStore.Application.Features.ProductVariants.Commands.AddVariant;
using OnlineStore.Application.Features.ProductVariants.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Mappings
{
    public class ProductVariantsProfile : Profile
    {
        public ProductVariantsProfile()
        {
            CreateMap<CreateVariantCommand, ProductVariant>();

            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(d => d.OriginalPrice, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images.OrderByDescending(i => i.IsMainImage).Select(i => i.ImageUrl).ToList()));


            CreateMap<ProductVariant, AdminVariantDto>()
                .ForMember(
                    dest => dest.OptionValues,
                    opt => opt.MapFrom(src => src.Options)
                )
                .ForMember(
                    dest => dest.OriginalPrice,
                    opt => opt.MapFrom(src => src.Price))
                .ForMember(
                    dest => dest.FinalPrice,
                    opt => opt.MapFrom(src => src.FinalPrice))
                .ForMember(dest => dest.HasDiscount,
                   opt => opt.MapFrom(src => src.Discount != null));


            CreateMap<VariantOption, VariantOptionValueDto>()
            .ForMember(
                dest => dest.OptionId,
                opt => opt.MapFrom(src => src.OptionId)
            )
            .ForMember(
                dest => dest.OptionName,
                opt => opt.MapFrom(src => src.Option.Name)
            )
            .ForMember(
                dest => dest.OptionValueId,
                opt => opt.MapFrom(src => src.OptionValueId)
            )
            .ForMember(
                dest => dest.Value,
                opt => opt.MapFrom(src => src.Value.Value)
            );



        }
    }
}
