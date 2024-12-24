using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Orders.Persistence;

public class OrdersDesignTimeDbContextFactory : DesignTimeDbContextFactory<OrdersDbContext>
{
    protected override string MigrationsHistoryTableName => OrdersDbContext.MigrationHistoryTableName;


    protected override OrdersDbContext CreateNewInstance(DbContextOptions<OrdersDbContext> options)
    {
        return new OrdersDbContext(options);
    }
}
