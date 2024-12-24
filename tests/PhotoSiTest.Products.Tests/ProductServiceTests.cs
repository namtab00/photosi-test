using AutoMapper;
using NSubstitute;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Domain.Mapping;
using PhotoSiTest.Products.Persistence;
using PhotoSiTest.Products.Services;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Products.Tests;

public class ProductServiceTests : ServiceTestBase
{
    public ProductServiceTests()
    {
        _productRepository = CreateSubstitute<IProductRepository>();
        _categoryRepository = CreateSubstitute<IProductCategoryRepository>();
        _productService = new ProductService(_productRepository, _categoryRepository, Mapper);
    }


    protected override void ConfigureMapper(IMapperConfigurationExpression config)
    {
        config.AddProfile<ProductMappingProfile>();
    }


    private readonly IProductService _productService;

    private readonly IProductRepository _productRepository;

    private readonly IProductCategoryRepository _categoryRepository;


    [Fact]
    public void AutoMapper_ProfileConfiguration_IsValid()
    {
        // Assert
        MapperConfiguration.AssertConfigurationIsValid();
    }


    [Fact]
    public async Task CreateProductAsync_WithInvalidCategory_ShouldThrow()
    {
        // Arrange
        var createDto = new CreateProductDto("Test", "Description", 10.00m, CategoryId: Guid.NewGuid());
        _categoryRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((ProductCategory?)null);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidEntityReferenceException>(async () => await _productService.CreateProductAsync(createDto));
        exception.EntityTypeName.ShouldBe(nameof(ProductCategory));
        exception.EntityId.ShouldBe(createDto.CategoryId);
    }


    [Fact]
    public async Task CreateProductAsync_WithValidCategory_ShouldSucceed()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var createDto = new CreateProductDto("Test", "Description", 10.00m, categoryId);
        _categoryRepository.GetByIdAsync(categoryId).Returns(new ProductCategory { Id = categoryId });
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(x => x.Arg<Product>());

        // Act
        var result = await _productService.CreateProductAsync(createDto);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(createDto.Name);
        result.CategoryId.ShouldBe(categoryId);
    }


    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Name = "Test", CategoryId = Guid.NewGuid() };
        _productRepository.GetByIdOrThrowAsync(id).Returns(product);

        // Act
        var result = await _productService.GetProductAsync(id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }
}
