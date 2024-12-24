namespace PhotoSiTest.Contracts.Domain.Orders.Dtos;

public record OrderDto(Guid Id, Guid UserId, Guid DeliveryAddressId, decimal TotalAmount, ICollection<OrderItemDto> Items);
