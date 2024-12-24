using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PhotoSiTest.Tests.Common;

public abstract class RepositoryTestBase<TContext> : IDisposable
    where TContext : DbContext
{
    protected readonly SqliteConnection Connection;

    protected readonly TContext Context;

    protected readonly DbContextOptions<TContext> Options;


    protected RepositoryTestBase()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        Options = new DbContextOptionsBuilder<TContext>().UseSqlite(Connection).Options;

        Context = CreateContext();
        Context.Database.EnsureCreated();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected abstract TContext CreateContext();


    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        Context.Database.EnsureDeleted();
        Context.Dispose();
        Connection.Dispose();
    }
}
