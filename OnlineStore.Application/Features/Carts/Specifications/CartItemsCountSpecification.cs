using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.CartsSpecifications
{
    public class CartItemsCountSpecification : BaseSpecification<CartItem>
    {
        public CartItemsCountSpecification(Guid userId)
        {
            Criteria = item => item.Cart.UserId == userId;
        }
    }
}
