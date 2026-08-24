using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Discounts.Commands.CreateDiscount;
using OnlineStore.Application.Features.Discounts.Commands.DeleteDiscount;
using OnlineStore.Application.Features.Discounts.Commands.DisableDiscount;
using OnlineStore.Application.Features.Discounts.Commands.EnableDiscount;
using OnlineStore.Application.Features.Discounts.Commands.UpdateDiscount;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Application.Features.Discounts.Queries.GetAllDiscounts;
using OnlineStore.Application.Features.Discounts.Queries.GetDiscountById;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class DiscountsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets a discount by its Id (Admin only).
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DiscountDto>> GetDiscountById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetDiscountByIdQuery(id), cancellationToken);
            return result;
        }

        /// <summary>
        /// Returns a paginated list of discounts (Admin only).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllDiscountsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }




        /// <summary>
        /// Creates a new discount
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(CreateDiscountCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetDiscountById), new { id = result }, null);
        }



        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DiscountDto>> UpdateDiscount(
            int id,
            [FromBody] UpdateDiscountCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        ///  Deletes the specified discount.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteDiscountCommand(id), cancellationToken);
            return NoContent();
        }


        ///
        /// <summary>Enables the specified discount.
        /// </summary>
        [HttpPatch("{id:int}/enable")]
        public async Task<IActionResult> EnableDiscount(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new EnableDiscountCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Disables the specified discount.
        /// </summary>
        [HttpPatch("{id:int}/disable")]
        public async Task<IActionResult> DisableDiscount(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DisableDiscountCommand(id), cancellationToken);
            return NoContent();
        }
    }
}