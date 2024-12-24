using Microsoft.Extensions.Options;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Common.Options;

namespace PhotoSiTest.API.HostedServices;

public class DataSeederHostedService(
    IServiceProvider serviceProvider,
    IOptions<PostgresOptions> postgresOptions,
    ILogger<DataSeederHostedService> logger,
    IHostEnvironment environment) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!postgresOptions.Value.EnableDataSeeding)
        {
            logger.LogWarning("skipping data seed");
            return;
        }

        if (!environment.IsDevelopment())
        {
            logger.LogWarning("skipping data seed for {EnvironmentName}", environment.EnvironmentName);
            return;
        }

        try
        {
            logger.LogInformation("Starting data seeding...");

            using var scope = serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync(cancellationToken);

            logger.LogInformation("Completed data seeding");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }


    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
