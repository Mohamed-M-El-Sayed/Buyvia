using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.Products.Commands.CreateProduct;
using OnlineStore.Application.Features.Products.Commands.DeleteProduct;
using OnlineStore.Application.Features.Products.Commands.PublishProduct;
using OnlineStore.Application.Features.Products.Commands.UnpublishProduct;
using OnlineStore.Application.Features.Products.Commands.UpdateProduct;
using OnlineStore.Application.Features.Products.Dtos;
using OnlineStore.Application.Features.Products.Queries.GetProductById;
using OnlineStore.Application.Features.Products.Queries.GetProductForEdit;
using OnlineStore.Application.Features.Products.Queries.GetProductsByCategory;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class ProductsController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Returns products belonging to the specified category.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("Category/{categoryId}")]
        [OutputCache(Tags = [CacheTags.Products], Duration = 300)]
        public async Task<IActionResult> GetProductsByCategory(int categoryId, [FromQuery] GetProductsByCategoryQuery query)
        {
            query.CategoryId = categoryId;
            var products = await mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Returns full product details including all variants and option selector data.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        /// <summary>
        /// Returns minimal product data for the admin edit form (Admin only).
        /// </summary>
        [HttpGet("{id:int}/edit")]
        public async Task<ActionResult<ProductEditDto>> GetProductForEdit([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetProductForEditQuery(id), cancellationToken);

            return Ok(result);
        }


        /// <summary>
        ///  Create a new product (Admin only).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// Update an existing product (Admin only).
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }
        /// <summary>
        /// Deletes the specified product and all its variants (Admin only).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Publishes a product after validating it is ready to go live (Admin only).
        /// </summary>
        [HttpPatch("{id:int}/publish")]
        public async Task<IActionResult> PublishProduct(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new PublishProductCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Unpublishes a product and sets it back to draft (Admin only).
        /// </summary>
        [HttpPatch("{id:int}/unpublish")]
        public async Task<IActionResult> UnpublishProduct(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new UnpublishProductCommand(id), cancellationToken);
            return NoContent();
        }

    }

}
