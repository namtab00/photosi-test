using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Common.Options;

namespace PhotoSiTest.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDbContext<TContext>(this IServiceCollection services,
        IConfiguration configuration,
        string migrationsHistoryTableName,
        string configSectionName = PostgresOptions.ConfigSectionName)
        where TContext : DbContext
    {
        var options = configuration.GetSection(configSectionName).Get<PostgresOptions>()
                      ?? throw new ApplicationException($"configuration section {PostgresOptions.ConfigSectionName} not found");

        services.AddScoped<IContextMigrator, ContextMigrator<TContext>>();

        services.AddDbContext<TContext>(dbContextOptions => {
            dbContextOptions.UseNpgsql(options.ConnectionString,
                optionsBuilder => {
                    optionsBuilder.MigrationsHistoryTable(migrationsHistoryTableName);
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                });
            dbContextOptions.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
        });

        return services;
    }
}
