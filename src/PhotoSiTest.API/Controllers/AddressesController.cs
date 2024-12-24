using Microsoft.AspNetCore.Mvc;
using PhotoSiTest.Contracts.Domain.Addresses;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;

namespace PhotoSiTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController(IAddressService addressService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddressDto>> Create(CreateAddressDto dto, CancellationToken ct = default)
    {
        var result = await addressService.CreateAddressAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await addressService.DeleteAddressAsync(id, ct);
        return NoContent();
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAll(CancellationToken ct = default) =>
        Ok(await addressService.GetAllAddressesAsync(ct));


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AddressDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await addressService.GetAddressAsync(id, ct);
        return Ok(result);
    }


    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetByUser(Guid userId, CancellationToken ct = default) =>
        Ok(await addressService.GetUserAddressesAsync(userId, ct));


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AddressDto>> Update(Guid id, UpdateAddressDto dto, CancellationToken ct = default)
    {
        var result = await addressService.UpdateAddressAsync(id, dto, ct);
        return Ok(result);
    }
}
