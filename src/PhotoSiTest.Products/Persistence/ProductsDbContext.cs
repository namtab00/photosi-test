using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Products.Domain;

namespace PhotoSiTest.Products.Persistence;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContextBase<ProductsDbContext>(options)
{
    public const string MigrationHistoryTableName = "__ProductsMigrationsHistory";

    public DbSet<ProductCategory> Categories => Set<ProductCategory>();

    public DbSet<Product> Products => Set<Product>();
}
