using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Products.Persistence;

public class ProductsDesignTimeDbContextFactory : DesignTimeDbContextFactory<ProductsDbContext>
{
    protected override string MigrationsHistoryTableName => ProductsDbContext.MigrationHistoryTableName;


    protected override ProductsDbContext CreateNewInstance(DbContextOptions<ProductsDbContext> options)
    {
        return new ProductsDbContext(options);
    }
}
