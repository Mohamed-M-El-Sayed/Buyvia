using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOption;
using OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOptionValue;
using OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOption;
using OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOptionValue;
using OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOption;
using OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOptionValue;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionById;
using OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionsByProductId;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId:int}/options")]
    public class ProductOptionsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets all options for the specified product.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ProductOptionDto>> GetByProductId(
            [FromRoute] int productId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetProductOptionsByProductIdQuery(productId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new option for the specified product (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromRoute] int productId,
            [FromBody] CreateProductOptionCommand command,
            CancellationToken cancellationToken)
        {
            command.ProductId = productId;

            var result = await mediator.Send(command, cancellationToken);

            return Ok(result);

        }

        /// <summary>
        /// Creates a new value for the specified option (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{optionId:int}/values")]
        public async Task<IActionResult> CreateValue(
            [FromRoute] int optionId,
            [FromBody] CreateProductOptionValueCommand command,
            CancellationToken cancellationToken)
        {
            command.OptionId = optionId;

            var result = await mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets an option by its identifier.
        /// </summary>
        [HttpGet("/api/product-options/{optionId:int}", Name = nameof(GetById))]
        public async Task<ActionResult<ProductOptionDetailsDto>> GetById([FromRoute] int optionId,
        CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetProductOptionByIdQuery(optionId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing product option (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("/api/product-options/{optionId:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int optionId,
            [FromBody] UpdateProductOptionCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = optionId;

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes a product option (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/product-options/{optionId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int optionId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteProductOptionCommand(optionId),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing option value (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("/api/product-option-values/{valueId:int}")]
        public async Task<IActionResult> UpdateValue(
            [FromRoute] int valueId,
            [FromBody] UpdateProductOptionValueCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = valueId;

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes an option value (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/product-option-values/{valueId:int}")]
        public async Task<IActionResult> DeleteValue(
            [FromRoute] int valueId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeleteProductOptionValueCommand(valueId),
                cancellationToken);

            return NoContent();
        }
    }
}