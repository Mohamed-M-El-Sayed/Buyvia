using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Specifications
{
    public class CartDetailsSpecification : BaseSpecification<Cart>
    {
        public CartDetailsSpecification(Guid userId, bool asNoTracking = true)
        {
            Criteria = cart => cart.UserId == userId;
            ApplyInclude(cart => cart.Items);
            ApplyInclude(cart => cart.Coupon!);
            ApplyInclude("Items.ProductVariant");
            ApplyInclude("Items.ProductVariant.Discount");
            ApplyInclude("Items.ProductVariant.Product");
            ApplyInclude("Items.ProductVariant.Options.Value");
            ApplyInclude("Items.ProductVariant.Images");
            if (asNoTracking)
                AsNoTracking();
        }
    }
}
