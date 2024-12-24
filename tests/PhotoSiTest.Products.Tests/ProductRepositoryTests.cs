using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Persistence;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Products.Tests;

public class ProductRepositoryTests : RepositoryTestBase<ProductsDbContext>
{
    public ProductRepositoryTests()
    {
        _repository = new ProductRepository(Context);
    }


    private readonly ProductRepository _repository;


    protected override ProductsDbContext CreateContext() => new(Options);


    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        // Arrange
        var category = new ProductCategory { Name = "Test Category" };
        await Context.Categories.AddAsync(category);
        await Context.SaveChangesAsync();

        var product = new Product {
            Name = "Test Product",
            Description = "Description",
            UnitPrice = 10.00m,
            CategoryId = category.Id
        };

        // Act
        var result = await _repository.AddAsync(product);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnProductWithCategory()
    {
        // Arrange
        var category = new ProductCategory { Name = "Test Category" };
        await Context.Categories.AddAsync(category);

        var product = new Product {
            Name = "Test Product",
            Description = "Description",
            UnitPrice = 10.00m,
            CategoryId = category.Id
        };
        await Context.Products.AddAsync(product);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Category.ShouldNotBeNull();
        result.Category.Id.ShouldBe(category.Id);
    }
}
