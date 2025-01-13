using PhotoSiTest.Common.Data;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Common.Options;
using PhotoSiTest.Users.API.Extensions.Swagger;
using PhotoSiTest.Users.Extensions;

namespace PhotoSiTest.Users.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices(builder.Configuration);
        return builder;
    }


    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.ConfigureSwagger();
        }

        app.UseExceptionHandler();

        app.MigrateDb();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }


    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogWarning("for env {EnvironmentName} applying migrations", app.Environment.EnvironmentName);

        var migrators = scope.ServiceProvider.GetServices<IContextMigrator>();

        foreach (var migrator in migrators)
        {
            logger.LogWarning("applying migrations for DbContext {DbContextTypeName}", migrator.ContextTypeName);
            migrator.Migrate();
        }
    }


    private static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PostgresOptions>()
            .Configure<IConfiguration>((options, conf) => conf.GetRequiredSection(PostgresOptions.ConfigSectionName).Bind(options));

        services.AddSwaggerServices().AddUsersDomainService(configuration).AddEndpointsApiExplorer();

        services.AddControllers();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddProblemDetails();
    }
}
