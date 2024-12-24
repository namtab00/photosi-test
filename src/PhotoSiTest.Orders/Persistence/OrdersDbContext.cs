using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Orders.Domain;

namespace PhotoSiTest.Orders.Persistence;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContextBase<OrdersDbContext>(options)
{
    public const string MigrationHistoryTableName = "__OrdersMigrationsHistory";

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Order> Orders => Set<Order>();
}
