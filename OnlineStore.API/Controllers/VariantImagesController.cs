using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.VariantImages.Command.AddVariantImages;
using OnlineStore.Application.Features.VariantImages.Command.DeleteVariantImage;
using OnlineStore.Application.Features.VariantImages.Command.SetMainVariantImage;
using OnlineStore.Application.Features.VariantImages.Dtos;
using OnlineStore.Application.Features.VariantImages.Queries.GetVariantImages;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [ApiController]
    [Route("api/variants/{variantId:int}/images")]
    public class VariantImagesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Adds one or more images to the specified variant (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddImages(
            [FromRoute] int variantId,
            [FromBody] AddVariantImagesCommand command,
            CancellationToken cancellationToken)
        {
            command.VariantId = variantId;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific image from the specified variant (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage(
            [FromRoute] int variantId,
            [FromRoute] int imageId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteVariantImageCommand(variantId, imageId),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Returns all images for the specified variant.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<VariantImageDto>>> GetImages(
            [FromRoute] int variantId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetVariantImagesQuery(variantId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Sets the specified image as the main image for the variant (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{imageId:int}/set-main")]
        public async Task<IActionResult> SetMainImage(
            [FromRoute] int variantId,
            [FromRoute] int imageId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new SetMainVariantImageCommand(variantId, imageId),
                cancellationToken);
            return NoContent();
        }
    }
}