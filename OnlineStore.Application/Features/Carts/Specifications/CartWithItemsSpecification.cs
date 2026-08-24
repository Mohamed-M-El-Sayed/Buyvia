using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;
namespace OnlineStore.Application.Features.Carts.Specifications
{
    public class CartWithItemsSpecification : BaseSpecification<Cart>
    {
        public CartWithItemsSpecification(Guid UserId)
        {
            Criteria = cart => cart.UserId == UserId;
            ApplyInclude(cart => cart.Items);
        }
    }
}
