using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.Brands.Commands.UpdateBrand;
using OnlineStore.Application.Features.Brands.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Mappings
{
    public class BrandsProfile : Profile
    {
        public BrandsProfile()
        {
            CreateMap<ProductBrand, BrandDto>()
                .ForMember(
                    dest => dest.LogoUrl,
                    opt => opt.ConvertUsing<ImageUrlConverter, string?>()
                );
            CreateMap<UpdateBrandCommand, ProductBrand>();
        }
    }
}
