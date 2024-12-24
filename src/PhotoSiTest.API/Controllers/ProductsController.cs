using Microsoft.AspNetCore.Mvc;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;

namespace PhotoSiTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto, CancellationToken ct = default)
    {
        var result = await productService.CreateProductAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await productService.DeleteProductAsync(id, ct);
        return NoContent();
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken ct = default) =>
        Ok(await productService.GetAllProductsAsync(ct));


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await productService.GetProductAsync(id, ct);
        return Ok(result);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var result = await productService.UpdateProductAsync(id, dto, ct);
        return Ok(result);
    }
}
