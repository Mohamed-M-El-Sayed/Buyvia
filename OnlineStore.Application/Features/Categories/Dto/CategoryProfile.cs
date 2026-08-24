using AutoMapper;
using OnlineStore.Application.Features.Categories.Commands.CreateCategory;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Dto
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ProductCategory, CategoryDto>();
            CreateMap<CreateCategoryCommand, ProductCategory>();
            CreateMap<ProductCategory, CategorySummaryDto>();
            CreateMap<ProductCategory, CategoryTreeDto>()
                .ForMember(dst => dst.Children, opt => opt.MapFrom(src => src.SubCategories));
        }
    }
}
