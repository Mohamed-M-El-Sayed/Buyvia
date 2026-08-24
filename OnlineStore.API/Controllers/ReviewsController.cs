using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.Reviews.Commands.CreateReview;
using OnlineStore.Application.Features.Reviews.Commands.DeleteReview;
using OnlineStore.Application.Features.Reviews.Commands.UpdateReview;
using OnlineStore.Application.Features.Reviews.Dtos;
using OnlineStore.Application.Features.Reviews.Queries.GetReviewById;
using OnlineStore.Application.Features.Reviews.Queries.GetReviewsByProduct;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReviewsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves a specific review by its Id
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReviewDto>> GetReviewById(
            int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetReviewByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of reviews for the specified product.
        /// </summary>
        [HttpGet("product/{productId:int}")]
        [OutputCache(Tags = [CacheTags.Reviews], Duration = 600)]
        [AllowAnonymous]
        public async Task<ActionResult<PageResult<ReviewDto>>> GetReviewsByProduct(
            [FromRoute] int productId,
            [FromQuery] GetReviewsByProductQuery query,
            CancellationToken cancellationToken)
        {
            query.ProductId = productId;
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new review for a product.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateReview(
            [FromBody] CreateReviewCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetReviewById), new { id = result }, null);
        }

        /// <summary>
        /// Updates the specified review. Only the review owner can update it.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReview(
            [FromRoute] int id,
            [FromBody] UpdateReviewCommand command,
            CancellationToken cancellationToken)
        {
            command.ReviewId = id;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Deletes the specified review. Users can only delete their own reviews.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteReviewCommand(id),
                cancellationToken);

            return NoContent();
        }
    }
}