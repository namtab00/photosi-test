using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Users.Domain;

namespace PhotoSiTest.Users.Persistence;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContextBase<UsersDbContext>(options)
{
    public const string MigrationHistoryTableName = "__UsersMigrationsHistory";

    public DbSet<User> Users => Set<User>();
}
