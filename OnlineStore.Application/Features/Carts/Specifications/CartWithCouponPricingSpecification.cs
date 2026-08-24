using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Specifications
{
    public class CartWithCouponPricingSpecification : BaseSpecification<Cart>
    {
        public CartWithCouponPricingSpecification(Guid userId, bool asNoTracking = false)
        {
            Criteria = cart => cart.UserId == userId;
            ApplyInclude(cart => cart.Items);
            ApplyInclude(cart => cart.Coupon!);
            ApplyInclude("Items.ProductVariant");
            ApplyInclude("Items.ProductVariant.Discount");

            if (asNoTracking)
                AsNoTracking();
        }
    }
}
