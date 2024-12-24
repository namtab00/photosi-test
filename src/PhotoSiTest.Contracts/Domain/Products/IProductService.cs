using PhotoSiTest.Contracts.Domain.Products.Dtos;

namespace PhotoSiTest.Contracts.Domain.Products;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default);


    Task DeleteProductAsync(Guid id, CancellationToken ct = default);


    Task<ProductDto?> FindProductAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken ct = default);


    Task<ProductDto> GetProductAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken ct = default);


    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default);
}
