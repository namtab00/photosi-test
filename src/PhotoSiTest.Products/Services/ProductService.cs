using AutoMapper;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Persistence;

namespace PhotoSiTest.Products.Services;

public class ProductService(IProductRepository productRepository, IProductCategoryRepository categoryRepository, IMapper mapper) : IProductService
{
    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        _ = await categoryRepository.GetByIdAsync(dto.CategoryId, ct) ?? throw new InvalidEntityReferenceException<ProductCategory>(dto.CategoryId);

        var product = mapper.Map<Product>(dto);
        var result = await productRepository.AddAsync(product, ct);
        return mapper.Map<ProductDto>(result);
    }


    public async Task DeleteProductAsync(Guid id, CancellationToken ct = default)
    {
        _ = await productRepository.GetByIdOrThrowAsync(id, ct);

        await productRepository.DeleteAsync(id, ct);
    }


    public async Task<ProductDto?> FindProductAsync(Guid id, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(id, ct);
        return product != null ? mapper.Map<ProductDto>(product) : null;
    }


    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken ct = default)
    {
        var products = await productRepository.GetListAsync(ct: ct);
        return mapper.Map<IEnumerable<ProductDto>>(products);
    }


    public async Task<ProductDto> GetProductAsync(Guid id, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdOrThrowAsync(id, ct);
        return mapper.Map<ProductDto>(product);
    }


    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        var products = await productRepository.GetListAsync(p => p.CategoryId == categoryId, ct: ct);
        return mapper.Map<IEnumerable<ProductDto>>(products.Where(p => p.CategoryId == categoryId));
    }


    public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdOrThrowAsync(id, ct);

        _ = await categoryRepository.GetByIdAsync(dto.CategoryId, ct) ?? throw new InvalidEntityReferenceException<ProductCategory>(dto.CategoryId);

        mapper.Map(dto, product);
        await productRepository.UpdateAsync(product, ct);
        return mapper.Map<ProductDto>(product);
    }
}
