namespace PhotoSiTest.Contracts.Domain.Orders.Dtos;

public record CreateOrderItemDto(Guid ProductId, int Quantity);
