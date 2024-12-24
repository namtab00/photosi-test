using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Products.Domain.Mapping;
using PhotoSiTest.Products.Persistence;
using PhotoSiTest.Products.Services;

namespace PhotoSiTest.Products.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductsService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ProductMappingProfile));
        services.AddPostgresDbContext<ProductsDbContext>(configuration, ProductsDbContext.MigrationHistoryTableName);
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        return services;
    }
}
