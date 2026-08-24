using AutoMapper;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Mappings
{
    public class ProductOptionProfile : Profile
    {
        public ProductOptionProfile()
        {
            CreateMap<ProductOption, ProductOptionDto>();
            CreateMap<ProductOption, ProductOptionDetailsDto>();
            CreateMap<ProductOptionValue, ProductOptionValueDto>();
        }
    }
}
