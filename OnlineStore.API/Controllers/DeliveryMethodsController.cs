using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod;
using OnlineStore.Application.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;
using OnlineStore.Application.Features.DeliveryMethods.Commands.SetDeliveryMethodActiveStatus;
using OnlineStore.Application.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;
using OnlineStore.Application.Features.DeliveryMethods.Queries.GetAllDeliveryMethods;
using OnlineStore.Application.Features.DeliveryMethods.Queries.GetAvailableDeliveryMethods;
using OnlineStore.Application.Features.DeliveryMethods.Queries.GetDeliveryById;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets all delivery methods. (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllDeliveryMethodsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets a delivery method by id. (Admin only)
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<DeliveryMethodDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetDeliveryByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new delivery method. (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(
            [FromBody] CreateDeliveryMethodCommand command,
            CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                null);
        }

        /// <summary>
        /// Updates an existing delivery method. (Admin only)
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDeliveryMethodCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Enables the specified delivery method. (Admin only)
        /// </summary>
        [HttpPatch("{id:int}/enable")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Enable(
            int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SetDeliveryMethodActiveStatusCommand(id, true),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Disables the specified delivery method. (Admin only)
        /// </summary>
        [HttpPatch("{id:int}/disable")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Disable(
            int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SetDeliveryMethodActiveStatusCommand(id, false),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes the specified delivery method. (Admin only)
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteDeliveryMethodCommand(id),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Gets all active delivery methods.
        /// </summary>
        [HttpGet("available")]
        [Authorize]
        public async Task<IActionResult> GetAvailable(
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetAvailableDeliveryMethodsQuery(),
                cancellationToken);

            return Ok(result);
        }
    }
}