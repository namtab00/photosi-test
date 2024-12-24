using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Addresses;
using PhotoSiTest.Contracts.Domain.Orders;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Orders.Domain;
using PhotoSiTest.Orders.Persistence;

namespace PhotoSiTest.Orders.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IMapper mapper,
    IUserService userService,
    IAddressService addressService,
    IProductService productService) : IOrderService
{
    public async Task<OrderDto> AddOrderItemAsync(Guid orderId, AddOrderItemDto dto, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, ct) ?? throw new EntityNotFoundException<Order>(orderId);

        var product = await productService.FindProductAsync(dto.ProductId, ct) ?? throw new InvalidEntityReferenceException("Product", dto.ProductId);

        var orderItem = new OrderItem {
            OrderId = orderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = product.UnitPrice
        };

        order.AddItem(orderItem);

        await orderRepository.UpdateAsync(order, ct);
        return mapper.Map<OrderDto>(order);
    }


    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct = default)
    {
        var user = await userService.FindUserAsync(dto.UserId, ct) ?? throw new InvalidEntityReferenceException("User", dto.UserId);

        var address = await addressService.FindAddressAsync(dto.DeliveryAddressId, ct)
                      ?? throw new InvalidEntityReferenceException("Address", dto.DeliveryAddressId);

        if (address.UserId != dto.UserId)
        {
            throw new InvalidOperationException($"Address {dto.DeliveryAddressId} does not belong to user {user.Id}");
        }

        var order = mapper.Map<Order>(dto);

        var result = await orderRepository.AddAsync(order, ct);
        return mapper.Map<OrderDto>(result);
    }


    public async Task DeleteOrderAsync(Guid id, CancellationToken ct = default)
    {
        _ = await orderRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<Order>(id);

        await orderRepository.DeleteAsync(id, ct);
    }


    public async Task<OrderDto?> FindOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByIdAsync(id, ct);
        return order != null ? mapper.Map<OrderDto>(order) : null;
    }


    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken ct = default)
    {
        var orders = await orderRepository.GetListAsync(include: q => q.Include(o => o.Items), ct: ct);
        return mapper.Map<IEnumerable<OrderDto>>(orders);
    }


    public async Task<OrderDto> GetOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<Order>(id);
        return mapper.Map<OrderDto>(order);
    }


    public async Task<OrderDto> RemoveOrderItemAsync(Guid orderId, Guid orderItemId, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, ct) ?? throw new EntityNotFoundException<Order>(orderId);

        order.RemoveItem(orderItemId);

        await orderRepository.UpdateAsync(order, ct);
        return mapper.Map<OrderDto>(order);
    }


    public async Task<OrderDto> UpdateOrderAsync(Guid id, UpdateOrderDto dto, CancellationToken ct = default)
    {
        var existingOrder = await orderRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<Order>(id);
        var user = await userService.FindUserAsync(dto.UserId, ct) ?? throw new InvalidEntityReferenceException("User", dto.UserId);

        var address = await addressService.FindAddressAsync(dto.DeliveryAddressId, ct)
                      ?? throw new InvalidEntityReferenceException("Address", dto.DeliveryAddressId);

        if (address.UserId != dto.UserId)
        {
            throw new InvalidOperationException($"Address {dto.DeliveryAddressId} does not belong to user {user.Id}");
        }

        mapper.Map(dto, existingOrder);

        await orderRepository.UpdateAsync(existingOrder, ct);
        return mapper.Map<OrderDto>(existingOrder);
    }
}
