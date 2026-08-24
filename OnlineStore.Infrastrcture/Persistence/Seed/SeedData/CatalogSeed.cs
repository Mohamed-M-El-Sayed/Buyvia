using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Infrastructure.Persistence.Seed.SeedData
{
    public class CatalogSeed
    {
        private readonly OnlineStoreDbContext _context;

        public CatalogSeed(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedCategoriesAsync();
            await SeedBrandsAsync();
            await SeedProductsAsync();
        }

        private async Task SeedCategoriesAsync()
        {
            if (await _context.Categories.AnyAsync())
                return;

            var electronics = new ProductCategory { Name = "Electronics" };
            var clothing = new ProductCategory { Name = "Clothing" };

            var laptops = new ProductCategory { Name = "Laptops", Parent = electronics };
            var phones = new ProductCategory { Name = "Phones", Parent = electronics };

            var gamingLaptops = new ProductCategory { Name = "Gaming Laptops", Parent = laptops };
            var ultrabooks = new ProductCategory { Name = "Ultrabooks", Parent = laptops };
            var androidPhones = new ProductCategory { Name = "Android Phones", Parent = phones };
            var iPhones = new ProductCategory { Name = "iPhones", Parent = phones };

            var men = new ProductCategory { Name = "Men", Parent = clothing };
            var women = new ProductCategory { Name = "Women", Parent = clothing };

            var menShirts = new ProductCategory { Name = "Shirts", Parent = men };
            var menPants = new ProductCategory { Name = "Pants", Parent = men };
            var womenDresses = new ProductCategory { Name = "Dresses", Parent = women };
            var womenShoes = new ProductCategory { Name = "Shoes", Parent = women };

            var menCasualShirts = new ProductCategory { Name = "Casual Shirts", Parent = menShirts };
            var menFormalShirts = new ProductCategory { Name = "Formal Shirts", Parent = menShirts };
            var womenHeels = new ProductCategory { Name = "Heels", Parent = womenShoes };
            var womenSneakers = new ProductCategory { Name = "Sneakers", Parent = womenShoes };

            await _context.Categories.AddRangeAsync(
                electronics, clothing,
                laptops, phones,
                gamingLaptops, ultrabooks, androidPhones, iPhones,
                men, women,
                menShirts, menPants, womenDresses, womenShoes,
                menCasualShirts, menFormalShirts, womenHeels, womenSneakers);

            await _context.SaveChangesAsync();
        }

        private async Task SeedBrandsAsync()
        {
            if (await _context.Brands.AnyAsync())
                return;

            await _context.Brands.AddRangeAsync(
                 new ProductBrand
                 {
                     Name = "HP",
                     LogoUrl = "Brand/HP.png"
                 },
                 new ProductBrand
                 {
                     Name = "Nike",
                     LogoUrl = "Brand/Nike.png"
                 });

            await _context.SaveChangesAsync();
        }

        private async Task SeedProductsAsync()
        {
            if (await _context.Products.AnyAsync())
                return;

            var gamingLaptops = await _context.Categories
                .FirstAsync(c => c.Name == "Gaming Laptops");

            var menCasualShirts = await _context.Categories
                .FirstAsync(c => c.Name == "Casual Shirts");

            var hp = await _context.Brands
                .FirstAsync(b => b.Name == "HP");

            var nike = await _context.Brands
                .FirstAsync(b => b.Name == "Nike");

            var simpleProduct = BuildSimpleProduct(
                gamingLaptops.Id,
                hp.Id);

            var productWithOptions = BuildProductWithOptions(
                menCasualShirts.Id,
                nike.Id);

            await _context.Products.AddRangeAsync(
                simpleProduct,
                productWithOptions);

            await _context.SaveChangesAsync();
        }
        // Simple product: no options, single default variant 
        private static Product BuildSimpleProduct(int categoryId, int brandId)
        {
            var product = new Product
            {
                Name = "Gaming Laptop X1",
                ShortDescription = "Entry-level gaming laptop",
                Description = "A solid entry-level gaming laptop with dedicated graphics.",
                CategoryId = categoryId,
                BrandId = brandId,
                Status = ProductStatus.Published
            };

            var variant = new ProductVariant
            {
                Price = 899.99m,
                Stock = 25,
                StockThreshold = 5,
                IsDefault = true,
                IsActive = true,
                Product = product
            };

            variant.Images.Add(new ProductImage
            {
                ImageUrl = "Product/Gaming-Laptop-X1.png",
                DisplayOrder = 0,
                IsMainImage = true,
            });

            product.Variants.Add(variant);
            return product;
        }

        // Product with options: Color x Size, one variant per combination 
        private static Product BuildProductWithOptions(int categoryId, int brandId)
        {
            var product = new Product
            {
                Name = "Classic Casual Shirt",
                ShortDescription = "Comfortable everyday shirt",
                Description = "A classic casual shirt available in multiple colors and sizes.",
                CategoryId = categoryId,
                BrandId = brandId,
                Status = ProductStatus.Published
            };

            var colorOption = new ProductOption
            {
                Name = "Color",
                Product = product
            };

            var red = new ProductOptionValue
            {
                Value = "Red",
                Option = colorOption
            };

            var blue = new ProductOptionValue
            {
                Value = "Blue",
                Option = colorOption
            };

            colorOption.Values.Add(red);
            colorOption.Values.Add(blue);

            var sizeOption = new ProductOption
            {
                Name = "Size",
                Product = product
            };

            var small = new ProductOptionValue
            {
                Value = "Small",
                Option = sizeOption
            };

            var medium = new ProductOptionValue
            {
                Value = "Medium",
                Option = sizeOption
            };

            sizeOption.Values.Add(small);
            sizeOption.Values.Add(medium);

            product.Options.Add(colorOption);
            product.Options.Add(sizeOption);

            var combinations = new (ProductOptionValue Color, ProductOptionValue Size, bool IsDefault)[]
            {
                (red, small, true),
                (red, medium, false),
                (blue, small, false),
                (blue, medium, false)
            };

            foreach (var (color, size, isDefault) in combinations)
            {
                var variant = new ProductVariant
                {
                    Price = 29.99m,
                    Stock = 50,
                    StockThreshold = 10,
                    IsDefault = isDefault,
                    IsActive = true,
                    Product = product
                };

                variant.Options.Add(new VariantOption
                {
                    Option = colorOption,
                    Value = color,
                    Variant = variant
                });

                variant.Options.Add(new VariantOption
                {
                    Option = sizeOption,
                    Value = size,
                    Variant = variant
                });

                var imageName = $"Product/Classic-Casual-Shirt-{color.Value}-{size.Value}.png";

                variant.Images.Add(new ProductImage
                {
                    ImageUrl = imageName,
                    DisplayOrder = 0,
                    IsMainImage = true,
                });

                product.Variants.Add(variant);
            }

            return product;
        }
    }
}