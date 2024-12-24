using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhotoSiTest.Common.Options;

namespace PhotoSiTest.Common.Data;

public sealed class ContextMigrator<T>(T context, IOptions<PostgresOptions> postgresOptions, ILogger<ContextMigrator<T>> logger) : IContextMigrator
    where T : DbContext
{
    public string ContextTypeName => typeof(T).Name;


    public void Migrate()
    {
        if (!postgresOptions.Value.EnableAutoMigration)
        {
            logger.LogWarning("skipping auto-migration");
            return;
        }

        try
        {
            logger.LogInformation("migrating {DbContextName}..", ContextTypeName);
            context.Database.Migrate();
            logger.LogInformation("{DbContextName} migrated !", ContextTypeName);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failure applying migrations for {context.GetType().Name}", e);
        }
    }
}
