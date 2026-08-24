using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Carts.Commands.AddItem;
using OnlineStore.Application.Features.Carts.Commands.ApplyCoupon;
using OnlineStore.Application.Features.Carts.Commands.ClearCart;
using OnlineStore.Application.Features.Carts.Commands.RemoveCartItem;
using OnlineStore.Application.Features.Carts.Commands.RemoveCoupon;
using OnlineStore.Application.Features.Carts.Commands.UpdateCartItem;
using OnlineStore.Application.Features.Carts.Dtos;
using OnlineStore.Application.Features.Carts.Queries.GetCart;
using OnlineStore.Application.Features.Carts.Queries.GetCartCount;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Adds an item to the user's shopping cart.
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddItem(
            [FromBody] AddToCartCommand command,
            CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets the current user's cart.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetCartQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets the total number of items in the current user's cart.
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCartCount(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetCartCountQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Updates the quantity of a specific item in the user's cart.
        /// </summary>
        [HttpPut("items/{productVariantId}")]
        public async Task<IActionResult> UpdateItem(
            int productVariantId,
            [FromBody] UpdateCartItemCommand command,
            CancellationToken cancellationToken)
        {
            command.ProductVariantId = productVariantId;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Removes a specific item from the user's cart.
        /// </summary>
        [HttpDelete("items/{productVariantId}")]
        public async Task<IActionResult> RemoveItem(
            int productVariantId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoveCartItemCommand(productVariantId), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Apply a coupon to the current user's cart
        /// </summary>
        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon(
            [FromBody] ApplyCouponCommand command,
            CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Removes the applied coupon from the current user's cart.
        /// </summary>
        [HttpPost("remove-coupon")]
        public async Task<IActionResult> RemoveCoupon(CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoveCouponCommand(), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            await mediator.Send(new ClearCartCommand(), cancellationToken);
            return NoContent();
        }
    }
}