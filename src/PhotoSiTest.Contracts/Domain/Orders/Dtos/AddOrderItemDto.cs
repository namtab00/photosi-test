namespace PhotoSiTest.Contracts.Domain.Orders.Dtos;

public record AddOrderItemDto(Guid ProductId, int Quantity);
