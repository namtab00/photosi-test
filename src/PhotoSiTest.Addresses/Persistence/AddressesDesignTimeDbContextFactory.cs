using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Addresses.Persistence;

public class AddressesDesignTimeDbContextFactory : DesignTimeDbContextFactory<AddressesDbContext>
{
    protected override string MigrationsHistoryTableName => AddressesDbContext.MigrationHistoryTableName;


    protected override AddressesDbContext CreateNewInstance(DbContextOptions<AddressesDbContext> options)
    {
        return new AddressesDbContext(options);
    }
}
