using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.Brands.Commands.CreateBrand;
using OnlineStore.Application.Features.Brands.Commands.DeleteBrand;
using OnlineStore.Application.Features.Brands.Commands.UpdateBrand;
using OnlineStore.Application.Features.Brands.Dtos;
using OnlineStore.Application.Features.Brands.Queries.GetAllBrands;
using OnlineStore.Application.Features.Brands.Queries.GetBrandById;
using OnlineStore.Domain.Constants;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class BrandsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns all brands.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [OutputCache(Duration = 600, Tags = [CacheTags.Brands])]
    public async Task<ActionResult<List<BrandDto>>> GetAll(CancellationToken cancellationToken)
    {
        var brands = await mediator.Send(new GetAllBrandsQuery(), cancellationToken);
        return Ok(brands);
    }

    /// <summary>
    /// Returns a single brand by its ID.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var brand = await mediator.Send(new GetBrandByIdQuery(id), cancellationToken);
        return Ok(brand);
    }

    /// <summary>
    /// Creates a new brand (Admin only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBrand(
        [FromBody] CreateBrandCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    /// <summary>
    /// Updates an existing brand (Admin only).
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBrand(
        [FromRoute] int id,
        [FromBody] UpdateBrandCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes the specified brand (Admin only).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrand(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteBrandCommand(id), cancellationToken);
        return NoContent();
    }
}