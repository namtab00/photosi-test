using Microsoft.EntityFrameworkCore;

namespace PhotoSiTest.Common.Data;

public abstract class DbContextBase<TContext>(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TContext).Assembly);
    }
}
