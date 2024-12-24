using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Users.Persistence;

public sealed class UsersDesignTimeDbContextFactory : DesignTimeDbContextFactory<UsersDbContext>
{
    protected override string MigrationsHistoryTableName => UsersDbContext.MigrationHistoryTableName;


    protected override UsersDbContext CreateNewInstance(DbContextOptions<UsersDbContext> options)
    {
        return new UsersDbContext(options);
    }
}
