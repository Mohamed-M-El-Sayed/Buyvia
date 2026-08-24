using AutoMapper;
using OnlineStore.Application.Features.Categories.Commands.CreateCategory;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ProductCategory, CategoryDto>();
            CreateMap<CreateCategoryCommand, ProductCategory>();
            CreateMap<ProductCategory, CategorySummaryDto>();


            // valid 
            CreateMap<ProductCategory, CategoryTreeDto>()
                .ForMember(dst => dst.Children, opt => opt.MapFrom(src => src.SubCategories));
        }
    }
}
