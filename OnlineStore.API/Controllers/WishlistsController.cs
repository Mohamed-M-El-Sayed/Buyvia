using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Wishlists.Commands.AddToWishlist;
using OnlineStore.Application.Features.Wishlists.Commands.ClearWishlist;
using OnlineStore.Application.Features.Wishlists.Commands.RemoveWishlistItem;
using OnlineStore.Application.Features.Wishlists.Dtos;
using OnlineStore.Application.Features.Wishlists.Queries.GetWishlist;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets the user's wishlist.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<WishlistDto>> GetWishlist(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWishlistQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Adds a product variant to the user's wishlist.
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddToWishlist(
            [FromBody] AddWishlistItemCommand command,
            CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Removes a product variant from the user's wishlist.
        /// </summary>
        [HttpDelete("items/{productVariantId:int}")]
        public async Task<IActionResult> DeleteItem(int productVariantId, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoveWishlistItemCommand(productVariantId), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Clears the user's wishlist.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearWishlist(CancellationToken cancellationToken)
        {
            await mediator.Send(new ClearWishlistCommand(), cancellationToken);
            return NoContent();
        }
    }
}
