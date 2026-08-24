using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.Coupons.Commands.CreateCoupon;
using OnlineStore.Application.Features.Coupons.Commands.DeleteCoupon;
using OnlineStore.Application.Features.Coupons.Commands.SetCouponActiveStatus;
using OnlineStore.Application.Features.Coupons.Commands.UpdateCoupon;
using OnlineStore.Application.Features.Coupons.Queries.GetAllCoupons;
using OnlineStore.Application.Features.Coupons.Queries.GetCouponById;
using OnlineStore.Domain.Constants;


namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class CouponsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Returns a paginated list of coupons (only admin).
        /// </summary>
        [HttpGet]
        [OutputCache(Duration = 300, Tags = [CacheTags.Coupons])]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCouponsQuery query, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new coupon (only admin).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CreateCouponCommand command, CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetCouponById), new { id }, null);
        }

        /// <summary>
        /// Gets a coupon by id (only admin).
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCouponById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var coupon = await mediator.Send(new GetCouponByIdQuery(id), cancellationToken);
            return Ok(coupon);
        }

        /// <summary>
        /// Updates an existing coupon (only admin).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon([FromRoute] int id, [FromBody] UpdateCouponCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Enables the specified coupon (only admin).
        /// </summary>
        [HttpPatch("{id:int}/enable")]
        public async Task<IActionResult> Enable(int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new SetCouponActiveStatusCommand(id, true), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Disables the specified coupon (only admin).
        /// </summary>
        [HttpPatch("{id:int}/disable")]
        public async Task<IActionResult> Disable(int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new SetCouponActiveStatusCommand(id, false), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a coupon by id (only admin).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteCouponCommand(id), cancellationToken);
            return NoContent();
        }

    }
}
