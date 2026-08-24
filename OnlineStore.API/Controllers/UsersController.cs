using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Auth.Dtos;
using OnlineStore.Application.Features.Users.Commands.AssignAdmin;
using OnlineStore.Application.Features.Users.Commands.UnassignAdmin;
using OnlineStore.Application.Features.Users.Queries.GetAllUsers;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Gets a paginated list of all users (Admin only).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] GetAllUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Assigns the Admin role to a user (Admin only).
        /// </summary>
        [HttpPost("{userId:guid}/assign-admin")]
        public async Task<ActionResult<MessageResponseDto>> AssignAdmin(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(
                new AssignAdminCommand { UserId = userId },
                cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Removes the Admin role from a user (Admin only).
        /// </summary>
        [HttpPost("{userId:guid}/unassign-admin")]
        public async Task<ActionResult<MessageResponseDto>> UnassignAdmin(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(
                new UnassignAdminCommand { UserId = userId },
                cancellationToken);

            return Ok(response);
        }

    }
}
