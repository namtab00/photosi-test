using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Addresses.Persistence;

public class AddressesDbContext(DbContextOptions<AddressesDbContext> options) : DbContextBase<AddressesDbContext>(options)
{
    public DbSet<Address> Addresses => Set<Address>();

    public static string MigrationHistoryTableName => "__AddressesMigrationsHistory";
}
