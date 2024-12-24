namespace PhotoSiTest.Contracts.Domain.Products.Dtos;

public record ProductDto(Guid Id, string Name, string Description, decimal UnitPrice, Guid CategoryId);
