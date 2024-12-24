using AutoMapper;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Persistence;

namespace PhotoSiTest.Products.Services;

public class ProductCategoryService(IProductCategoryRepository categoryRepository, IMapper mapper) : IProductCategoryService
{
    public async Task<ProductCategoryDto> CreateCategoryAsync(CreateProductCategoryDto dto, CancellationToken ct = default)
    {
        var conflict = await GetCategoryByNameAsync(dto.Name, ct);
        if (conflict != null)
        {
            throw new InvalidOperationException($"Category with name '{dto.Name}' already exists, id '{conflict.Id}'");
        }

        var category = mapper.Map<ProductCategory>(dto);
        var result = await categoryRepository.AddAsync(category, ct);
        return mapper.Map<ProductCategoryDto>(result);
    }


    public async Task DeleteCategoryAsync(Guid id, CancellationToken ct = default)
    {
        _ = await categoryRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<ProductCategory>(id);
        await categoryRepository.DeleteAsync(id, ct);
    }


    public async Task<ProductCategoryDto?> FindCategoryAsync(Guid id, CancellationToken ct = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, ct);
        return category != null ? mapper.Map<ProductCategoryDto>(category) : null;
    }


    public async Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default)
    {
        var categories = await categoryRepository.GetListAsync(ct: ct);
        return mapper.Map<IEnumerable<ProductCategoryDto>>(categories);
    }


    public async Task<ProductCategoryDto> GetCategoryAsync(Guid id, CancellationToken ct = default)
    {
        var category = await categoryRepository.GetByIdOrThrowAsync(id, ct);
        return mapper.Map<ProductCategoryDto>(category);
    }


    public async Task<ProductCategoryDto?> GetCategoryByNameAsync(string categoryName, CancellationToken ct = default)
    {
        var categories = await categoryRepository.GetListAsync(c => c.Name == categoryName, ct: ct);
        var category = categories.FirstOrDefault();
        return category != null ? mapper.Map<ProductCategoryDto>(category) : null;
    }


    public async Task<ProductCategoryDto> UpdateCategoryAsync(Guid id, UpdateProductCategoryDto dto, CancellationToken ct = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<ProductCategory>(id);

        mapper.Map(dto, category);
        await categoryRepository.UpdateAsync(category, ct);
        return mapper.Map<ProductCategoryDto>(category);
    }
}
