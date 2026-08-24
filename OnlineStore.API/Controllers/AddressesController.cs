using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OnlineStore.API.Extensions;
using OnlineStore.Application.Features.Addresses.Commands.CreateAddress;
using OnlineStore.Application.Features.Addresses.Commands.DeleteAddress;
using OnlineStore.Application.Features.Addresses.Commands.SendVerification;
using OnlineStore.Application.Features.Addresses.Commands.SetDefaultAddress;
using OnlineStore.Application.Features.Addresses.Commands.UpdateAddress;
using OnlineStore.Application.Features.Addresses.Commands.VerifyPhone;
using OnlineStore.Application.Features.Addresses.Dtos;
using OnlineStore.Application.Features.Addresses.Queries.GetAddressById;
using OnlineStore.Application.Features.Addresses.Queries.GetMyAddresses;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    /// <summary>
    /// Manages user addresses.
    /// </summary>
    [ApiController]
    [Authorize(Roles = Roles.Customer)]
    [Route("api/[controller]")]
    public class AddressesController(IMediator mediator) : ControllerBase
    {





        /// <summary>
        /// Gets all addresses for the current user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<AddressDto>>> GetMyAddresses(
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetMyAddressesQuery(),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets an address by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AddressDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetAddressByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new address.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateAddressCommand command,
            CancellationToken cancellationToken)
        {
            var id = await mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                null);
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateAddressCommand command,
            CancellationToken cancellationToken)
        {
            command.AddressId = id;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes an address.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteAddressCommand(id),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Sets an address as the default address.
        /// </summary>
        [HttpPatch("{id:int}/default")]
        public async Task<IActionResult> SetDefault(
            int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SetDefaultAddressCommand(id),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Sends a phone verification code.
        /// </summary>
        [HttpPost("{addressId:int}/send-verification")]
        [EnableRateLimiting(RateLimitingExtensions.Otp)]
        public async Task<IActionResult> SendVerification(
            int addressId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SendVerificationCommand(addressId),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Verifies the address phone number using an OTP code.
        /// </summary>
        [HttpPost("{addressId:int}/verify-phone")]
        public async Task<IActionResult> VerifyPhone(
            int addressId,
            VerifyPhoneCommand command,
            CancellationToken cancellationToken)
        {
            command.AddressId = addressId;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }
    }
}