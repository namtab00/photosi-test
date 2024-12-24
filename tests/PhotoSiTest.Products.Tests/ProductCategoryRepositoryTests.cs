using PhotoSiTest.Products.Domain;
using PhotoSiTest.Products.Persistence;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Products.Tests;

public class ProductCategoryRepositoryTests : RepositoryTestBase<ProductsDbContext>
{
    public ProductCategoryRepositoryTests()
    {
        _repository = new ProductCategoryRepository(Context);
    }


    private readonly ProductCategoryRepository _repository;


    protected override ProductsDbContext CreateContext() => new(Options);


    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        var category = new ProductCategory {
            Name = "Test Category",
            Description = "Description"
        };

        // Act
        var result = await _repository.AddAsync(category);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);

        var dbCategory = await Context.Categories.FindAsync(result.Id);
        dbCategory.ShouldNotBeNull();
    }


    [Fact]
    public async Task GetByIdAsync_WithProducts_ShouldIncludeProducts()
    {
        // Arrange
        var category = new ProductCategory { Name = "Test Category" };
        await Context.Categories.AddAsync(category);

        var product = new Product {
            Name = "Test Product",
            CategoryId = category.Id
        };
        await Context.Products.AddAsync(product);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(category.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Products.ShouldNotBeEmpty();
        result.Products.First().Id.ShouldBe(product.Id);
    }
}
