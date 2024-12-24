namespace PhotoSiTest.Contracts.Domain.Orders.Dtos;

public record CreateOrderDto(Guid UserId, Guid DeliveryAddressId);
