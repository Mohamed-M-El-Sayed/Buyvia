using AutoMapper;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Features.Products.Commands.CreateProduct;
using OnlineStore.Application.Features.Products.Commands.UpdateProduct;
using OnlineStore.Application.Features.Products.Dtos;
using OnlineStore.Application.Features.ProductVariants.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Mappings
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {

            CreateMap<ProductVariant, ProductSummaryDto>()
               .ForMember(dest => dest.Id,
                   opt => opt.MapFrom(src => src.Product.Id))
               .ForMember(dest => dest.DefaultVariantId,
                   opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ProductName,
                   opt => opt.MapFrom(src => src.Product.Name))
               .ForMember(dest => dest.ShortDescription,
                   opt => opt.MapFrom(src => src.Product.ShortDescription))
               .ForMember(dest => dest.BrandName,
                   opt => opt.MapFrom(src => src.Product.Brand.Name))
               .ForMember(dest => dest.BrandId,
                    opt => opt.MapFrom(src => src.Product.BrandId))
               .ForMember(dest => dest.CategoryName,
                   opt => opt.MapFrom(src => src.Product.Category.Name))
               .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.Product.CategoryId))
               .ForMember(
                    dest => dest.MainImageUrl,
                    opt => opt.ConvertUsing<ImageUrlConverter, string?>(
                        src => src.Images
                            .Where(i => i.IsMainImage)
                            .Select(i => i.ImageUrl)
                            .FirstOrDefault()
                    ))
               .ForMember(dest => dest.OriginalPrice,
                   opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.FinalPrice,
               opt => opt.MapFrom(src => src.FinalPrice))
               .ForMember(dest => dest.HasDiscount,
                   opt => opt.MapFrom(src => src.Discount != null))
               .ForMember(dest => dest.InStock,
                   opt => opt.MapFrom(src => src.Stock > 0));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Variants, opt => opt.Ignore());


            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());




            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));



            CreateMap<Product, ProductDetailsDto>()
                 .ForMember(dest => dest.BrandName, opt => opt.MapFrom(s => s.Brand.Name))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
                 .ForMember(dest => dest.IsSimple, opt => opt.MapFrom(s => !s.Options.Any()));
            CreateMap<ProductOption, ProductOptionDto>();
            CreateMap<ProductOptionValue, ProductOptionValueDto>();

            CreateMap<Product, ProductEditDto>();

            CreateMap<ProductVariant, ProductVariantDto>()
            .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(s => s.Price))
            .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(s => s.FinalPrice))
            .ForMember(dest => dest.HasDiscount, opt => opt.Ignore())
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(s => s.Stock > 0))
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(s => s.GetVariantName()))
            .ForMember(
                dest => dest.Images,
                opt => opt.ConvertUsing<ProductImagesToVariantImageDtosConverter, ICollection<ProductImage>>(
                    s => s.Images))
            .ForMember(dest => dest.OptionValueIds,
                opt => opt.MapFrom(s => s.Options.Select(o => o.OptionValueId).ToList()));
        }
    }
}
