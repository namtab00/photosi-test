using Microsoft.AspNetCore.Mvc;
using PhotoSiTest.Contracts.Domain.Orders;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;

namespace PhotoSiTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost("{orderId:guid}/items")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> AddOrderItem(Guid orderId, AddOrderItemDto dto, CancellationToken ct = default)
    {
        var result = await orderService.AddOrderItemAsync(orderId, dto, ct);
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto, CancellationToken ct = default)
    {
        var result = await orderService.CreateOrderAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await orderService.DeleteOrderAsync(id, ct);
        return NoContent();
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll(CancellationToken ct = default)
    {
        var orders = await orderService.GetAllOrdersAsync(ct);
        return Ok(orders);
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await orderService.GetOrderAsync(id, ct);
        return Ok(result);
    }


    [HttpDelete("{orderId:guid}/items/{itemId:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> RemoveOrderItem(Guid orderId, Guid itemId, CancellationToken ct = default)
    {
        var result = await orderService.RemoveOrderItemAsync(orderId, itemId, ct);
        return Ok(result);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> Update(Guid id, UpdateOrderDto dto, CancellationToken ct = default)
    {
        var result = await orderService.UpdateOrderAsync(id, dto, ct);
        return Ok(result);
    }
}
