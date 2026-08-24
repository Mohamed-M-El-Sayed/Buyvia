using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.Categories.Commands.CreateCategory;
using OnlineStore.Application.Features.Categories.Commands.DeleteCategory;
using OnlineStore.Application.Features.Categories.Commands.UpdateCategory;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Application.Features.Categories.Queries.GetCategoryById;
using OnlineStore.Application.Features.Categories.Queries.GetCategoryTree;
using OnlineStore.Application.Features.Categories.Queries.GetLeafCategories;
using OnlineStore.Application.Features.Categories.Queries.GetTopLevelCategories;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Returns top-level categories (no parent).
        /// </summary>    
        [OutputCache(Duration = 600, Tags = [CacheTags.Categories])]
        [HttpGet("root")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopLevelCategories(CancellationToken cancellationToken)
        {
            var categories = await mediator.Send(new GetTopLevelCategoriesQuery(), cancellationToken);
            return Ok(categories);
        }


        /// <summary>
        /// Returns the full category tree including subcategories.
        /// </summary>
        [HttpGet("tree")]
        [OutputCache(Duration = 600, Tags = [CacheTags.Categories])]
        [AllowAnonymous]
        public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
        {
            var categories = await mediator.Send(new GetCategoryTreeQuery(), cancellationToken);
            return Ok(categories);
        }

        /// <summary>
        /// Gets all leaf categories under the specified root category.
        /// </summary>
        [HttpGet("{rootId:int}/leaves")]
        [OutputCache(Duration = 600, Tags = [CacheTags.Categories])]
        [AllowAnonymous]
        public async Task<ActionResult> GetLeaves(
            int rootId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetLeafCategoriesQuery(rootId),
                cancellationToken);

            return Ok(result);
        }


        /// <summary>
        /// Gets a category by id.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));
            return category;
        }

        /// <summary>
        /// Creates a new category (Admin only).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var categoryId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, null);
        }

        /// <summary>
        /// Updates an existing category (Admin only).
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Delete a category by id (Admin only).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return NoContent();
        }


    }
}
