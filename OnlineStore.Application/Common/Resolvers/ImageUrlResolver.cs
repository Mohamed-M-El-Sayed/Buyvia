using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineStore.Application.Features.VariantImages.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Common.Resolvers
{
    public class ImageUrlConverter : IValueConverter<string?, string>
    {
        private readonly string _baseUrl;

        public ImageUrlConverter(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? string.Empty;
        }

        public string Convert(string? sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember))
                return "default.png";
            return $"{_baseUrl.TrimEnd('/')}/{sourceMember.TrimStart('/')}";
        }
    }

    public class ImageUrlsConverter : IValueConverter<IEnumerable<string?>, List<string>>
    {
        private readonly string _baseUrl;

        public ImageUrlsConverter(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? string.Empty;
        }

        public List<string> Convert(IEnumerable<string?> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null) return new List<string>();
            return sourceMember
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"{_baseUrl.TrimEnd('/')}/{x!.TrimStart('/')}")
                .ToList();
        }
    }

    public class ProductImagesToVariantImageDtosConverter
    : IValueConverter<ICollection<ProductImage>, List<VariantImageDto>>
    {
        private readonly string _baseUrl;

        public ProductImagesToVariantImageDtosConverter(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? string.Empty;
        }

        public List<VariantImageDto> Convert(
              ICollection<ProductImage> sourceMember,
              ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
                return new List<VariantImageDto>();

            return sourceMember
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new VariantImageDto
                {
                    ImageUrl = $"{_baseUrl.TrimEnd('/')}/{i.ImageUrl.TrimStart('/')}",
                    IsMainImage = i.IsMainImage,
                    DisplayOrder = i.DisplayOrder
                })
                .ToList();
        }
    }
}