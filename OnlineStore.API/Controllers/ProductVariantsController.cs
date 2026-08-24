using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.ProductVariants.Commands.AddVariant;
using OnlineStore.Application.Features.ProductVariants.Commands.AssignDiscountToVariant;
using OnlineStore.Application.Features.ProductVariants.Commands.BulkCreateVariants;
using OnlineStore.Application.Features.ProductVariants.Commands.BulkUpdateVariants;
using OnlineStore.Application.Features.ProductVariants.Commands.DeleteVariant;
using OnlineStore.Application.Features.ProductVariants.Commands.SetDefaultVariant;
using OnlineStore.Application.Features.ProductVariants.Commands.UnassignDiscountFromVariant;
using OnlineStore.Application.Features.ProductVariants.Commands.UpdateVariant;
using OnlineStore.Application.Features.ProductVariants.Dtos;
using OnlineStore.Application.Features.ProductVariants.Queries.GetVariantById;
using OnlineStore.Application.Features.ProductVariants.Queries.GetVariantsByProductId;
using OnlineStore.Domain.Constants;
namespace OnlineStore.API.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/products/{productId}/variants")]
    [ApiController]
    public class ProductVariantsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Returns all variants for the specified product. (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVariants(
            [FromRoute] int productId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetVariantsByProductIdQuery(productId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Creates a simple variant (no options) for the specified product. (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateVariant(
            [FromRoute] int productId,
            [FromBody] CreateVariantCommand command,
            CancellationToken cancellationToken)
        {
            command.ProductId = productId;

            var id = await mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetVariantById),
                new { id },
                null);
        }

        /// <summary>
        /// Generates all option combinations as variants for the specified product. (Admin only)
        /// </summary>
        [HttpPost("bulk")]
        public async Task<ActionResult<List<int>>> BulkCreateVariants(
            [FromRoute] int productId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new BulkCreateVariantsCommand(productId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a single variant by its ID. (Admin only)
        /// </summary>
        [HttpGet("/api/variants/{id:int}")]
        public async Task<ActionResult<ProductVariantDto>> GetVariantById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetVariantByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates price, stock, and availability for the specified variant. (Admin only)
        /// </summary>
        [HttpPut("/api/variants/{id:int}")]
        public async Task<IActionResult> UpdateVariant(
            [FromRoute] int id,
            [FromBody] UpdateVariantCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Updates price, stock, and availability for multiple variants in one transaction. (Admin only)
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> BulkUpdateVariants(
            [FromRoute] int productId,
            [FromBody] BulkUpdateVariantsCommand command,
            CancellationToken cancellationToken)
        {
            command.ProductId = productId;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Sets the specified variant as the default for its product. (Admin only)
        /// </summary>
        [HttpPatch("/api/variants/{id:int}/set-default")]
        public async Task<IActionResult> SetDefaultVariant(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SetDefaultVariantCommand(id),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Soft-deletes the specified variant. (Admin only)
        /// </summary>
        [HttpDelete("/api/variants/{id:int}")]
        public async Task<IActionResult> DeleteVariant(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteVariantCommand(id),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Assigns an existing discount to the specified variant. (Admin only)
        /// </summary>
        [HttpPost("/api/variants/{variantId:int}/discounts/{discountId:int}")]
        public async Task<IActionResult> AssignDiscount(
            [FromRoute] int variantId,
            [FromRoute] int discountId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new AssignDiscountToVariantCommand(
                    variantId,
                    discountId),
                cancellationToken);

            return NoContent();
        }
        /// <summary>
        /// Removes the assigned discount from the specified variant (Admin only).
        /// </summary>
        [HttpDelete("/api/variants/{variantId:int}/discount")]
        public async Task<IActionResult> UnassignDiscount(
            [FromRoute] int variantId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new UnassignDiscountFromVariantCommand(variantId),
                cancellationToken);

            return NoContent();
        }

    }
}
