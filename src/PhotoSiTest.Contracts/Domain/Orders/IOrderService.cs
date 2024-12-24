using PhotoSiTest.Contracts.Domain.Orders.Dtos;

namespace PhotoSiTest.Contracts.Domain.Orders;

public interface IOrderService
{
    Task<OrderDto> AddOrderItemAsync(Guid orderId, AddOrderItemDto dto, CancellationToken ct = default);


    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct = default);


    Task DeleteOrderAsync(Guid id, CancellationToken ct = default);


    Task<OrderDto?> FindOrderAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken ct = default);


    Task<OrderDto> GetOrderAsync(Guid id, CancellationToken ct = default);


    Task<OrderDto> RemoveOrderItemAsync(Guid orderId, Guid orderItemId, CancellationToken ct = default);


    Task<OrderDto> UpdateOrderAsync(Guid id, UpdateOrderDto dto, CancellationToken ct = default);
}
