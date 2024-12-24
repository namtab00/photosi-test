using Microsoft.AspNetCore.Mvc;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;

namespace PhotoSiTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController(IProductCategoryService categoryService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductCategoryDto>> Create(CreateProductCategoryDto dto, CancellationToken ct = default)
    {
        var result = await categoryService.CreateCategoryAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await categoryService.DeleteCategoryAsync(id, ct);
        return NoContent();
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetAll(CancellationToken ct = default) =>
        Ok(await categoryService.GetAllCategoriesAsync(ct));


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductCategoryDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await categoryService.GetCategoryAsync(id, ct);
        return Ok(result);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductCategoryDto>> Update(Guid id, UpdateProductCategoryDto dto, CancellationToken ct = default)
    {
        var result = await categoryService.UpdateCategoryAsync(id, dto, ct);
        return Ok(result);
    }
}
