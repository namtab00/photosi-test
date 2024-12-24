using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PhotoSiTest.Common.Options;

namespace PhotoSiTest.Common.Data;

public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContextBase<TContext>
{
    private static Assembly MigrationsAssembly => typeof(TContext).Assembly;

    protected abstract string MigrationsHistoryTableName { get; }


    public TContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

        var options = configuration.GetSection(PostgresOptions.ConfigSectionName).Get<PostgresOptions>()
                      ?? throw new ApplicationException($"configuration section {PostgresOptions.ConfigSectionName} not found");

        optionsBuilder.UseNpgsql(options.ConnectionString,
            opt => {
                opt.MigrationsHistoryTable(MigrationsHistoryTableName);
                opt.MigrationsAssembly(MigrationsAssembly);
            });

        return CreateNewInstance(optionsBuilder.Options);
    }


    protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);


    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets(MigrationsAssembly);

        return builder.Build();
    }
}
