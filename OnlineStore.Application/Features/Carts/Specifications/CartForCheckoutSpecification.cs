using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Specifications
{
    public class CartForCheckoutSpecification : BaseSpecification<Cart>
    {
        public CartForCheckoutSpecification(Guid userId)
        {
            Criteria = cart => cart.UserId == userId;

            ApplyInclude(c => c.Items);
            ApplyInclude("Items.ProductVariant");
            ApplyInclude("Items.ProductVariant.Discount");
            ApplyInclude("Items.ProductVariant.Product");
            ApplyInclude("Items.ProductVariant.Images");
            ApplyInclude("Items.ProductVariant.Options.Value");
            ApplyInclude(c => c.Coupon!);
        }
    }
}