using AutoMapper;
using NSubstitute;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Domain.Mapping;
using PhotoSiTest.Products.Persistence;
using PhotoSiTest.Products.Services;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Products.Tests;

public class ProductCategoryServiceTests : ServiceTestBase
{
    public ProductCategoryServiceTests()
    {
        _categoryRepository = CreateSubstitute<IProductCategoryRepository>();
        _categoryService = new ProductCategoryService(_categoryRepository, Mapper);
    }


    private readonly ProductCategoryService _categoryService;

    private readonly IProductCategoryRepository _categoryRepository;


    protected override void ConfigureMapper(IMapperConfigurationExpression config)
    {
        config.AddProfile<ProductMappingProfile>();
    }


    [Fact]
    public async Task CreateCategoryAsync_ShouldSucceed()
    {
        // Arrange
        var createDto = new CreateProductCategoryDto("Test", "Description");
        _categoryRepository.AddAsync(Arg.Any<ProductCategory>()).Returns(x => x.Arg<ProductCategory>());

        // Act
        var result = await _categoryService.CreateCategoryAsync(createDto);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(createDto.Name);
    }


    [Fact]
    public async Task UpdateCategoryAsync_WithExistingCategory_ShouldSucceed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var category = new ProductCategory { Id = id, Name = "Old Name" };
        var updateDto = new UpdateProductCategoryDto("New Name", "New Description");
        _categoryRepository.GetByIdAsync(id).Returns(category);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(id, updateDto);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(updateDto.Name);
    }
}
