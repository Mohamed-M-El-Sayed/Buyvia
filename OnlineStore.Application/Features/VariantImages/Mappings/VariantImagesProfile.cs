using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.VariantImages.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Mappings
{
    public class VariantImagesProfile : Profile
    {
        public VariantImagesProfile()
        {
            CreateMap<ProductImage, VariantImageDto>()
                            .ForMember(
                                dest => dest.ImageUrl,
                                opt => opt.ConvertUsing<ImageUrlConverter, string?>(src => src.ImageUrl));
        }

    }
}
