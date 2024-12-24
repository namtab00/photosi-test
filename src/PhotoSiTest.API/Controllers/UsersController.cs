using Microsoft.AspNetCore.Mvc;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;

namespace PhotoSiTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto, CancellationToken ct = default)
    {
        var result = await userService.CreateUserAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await userService.DeleteUserAsync(id, ct);
        return NoContent();
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken ct = default) => Ok(await userService.GetAllUsersAsync(ct));


    [HttpGet("by-email")]
    public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email, CancellationToken ct = default)
    {
        var user = await userService.GetUserByEmailAsync(email, ct);
        return user == null ? NotFound() : Ok(user);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await userService.GetUserAsync(id, ct);
        return Ok(result);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, UpdateUserDto dto, CancellationToken ct = default)
    {
        var result = await userService.UpdateUserAsync(id, dto, ct);
        return Ok(result);
    }
}
