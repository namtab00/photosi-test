namespace PhotoSiTest.Contracts.Domain.Orders.Dtos;

public record OrderItemDto(Guid Id, Guid ProductId, int Quantity, decimal UnitPrice);
