using PhotoSiTest.Contracts.Domain.Products.Dtos;

namespace PhotoSiTest.Contracts.Domain.Products;

public interface IProductCategoryService
{
    Task<ProductCategoryDto> CreateCategoryAsync(CreateProductCategoryDto dto, CancellationToken ct = default);


    Task DeleteCategoryAsync(Guid id, CancellationToken ct = default);


    Task<ProductCategoryDto?> FindCategoryAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default);


    Task<ProductCategoryDto> GetCategoryAsync(Guid id, CancellationToken ct = default);


    Task<ProductCategoryDto?> GetCategoryByNameAsync(string categoryName, CancellationToken ct = default);


    Task<ProductCategoryDto> UpdateCategoryAsync(Guid id, UpdateProductCategoryDto dto, CancellationToken ct = default);
}
