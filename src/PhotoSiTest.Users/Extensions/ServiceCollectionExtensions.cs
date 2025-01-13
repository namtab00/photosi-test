using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoSiTest.Common.API;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Common.Options;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Users.Domain.Mapping;
using PhotoSiTest.Users.Persistence;
using PhotoSiTest.Users.Services;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Refit;

namespace PhotoSiTest.Users.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly IEnumerable<TimeSpan> _retryDelays = Backoff.DecorrelatedJitterBackoffV2(
        medianFirstRetryDelay: TimeSpan.FromMilliseconds(500),
        retryCount: 10);


    public static IServiceCollection AddUsersProxy(this IServiceCollection services, IHostEnvironment hostEnvironment)
    {
        services.AddScoped<IUserServiceProxy, UserServiceProxy>();

        return services.AddServicesConfigOptions()
            .AddRefitForUsersApi(hostEnvironment,
                (sp, httpClient) => {
                    var servicesOptions = sp.GetRequiredOptions<ServicesConfigOptions>().Value;
                    httpClient.BaseAddress = servicesOptions.GetUsersApiBaseUriOrThrow();
                    httpClient.Timeout = servicesOptions.TimeoutSeconds.HasValue
                        ? TimeSpan.FromSeconds(servicesOptions.TimeoutSeconds.Value)
                        : TimeSpan.FromMinutes(5);
                });
    }


    public static IServiceCollection AddUsersDomainService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddPostgresDbContext<UsersDbContext>(configuration, UsersDbContext.MigrationHistoryTableName);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserDomainService, UserDomainService>();

        return services;
    }


    private static IServiceCollection AddRefitForUsersApi(this IServiceCollection services,
        IHostEnvironment hostEnvironment,
        Action<IServiceProvider, HttpClient> configureClient)
    {
        var clientBuilder = services.AddRefitClient<IUsersApiClient>(_ => {
                var serializerOptions = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();

                // removes Refit enum to string serialization
                serializerOptions.Converters.Clear();
                serializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                return new RefitSettings {
                    ContentSerializer = new SystemTextJsonContentSerializer(serializerOptions),
                    CollectionFormat = CollectionFormat.Multi,
                    Buffered = false
                };
            })
            .AddPolicyHandler((serviceProvider, _) => Policy<HttpResponseMessage>.Handle<ApiException>()
                .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.NotFound)
                .WaitAndRetryAsync(_retryDelays, OnRetry(serviceProvider)))
            .ConfigureHttpClient(configureClient);

        var shouldLogRequest = hostEnvironment.IsDevelopment();
        clientBuilder.AddHttpMessageHandler(sp => {
            var logger = sp.GetRequiredService<ILogger<CustomLoggingHttpMessageHandler>>();
            return new CustomLoggingHttpMessageHandler(logger, logRequest: shouldLogRequest, logResponse: true);
        });

        return services;

        Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> OnRetry(IServiceProvider serviceProvider) =>
            (result, timeSpan, retryCount, _) => {
                var logger = serviceProvider.GetService<ILogger<CustomLoggingHttpMessageHandler>>();
                if (result.Exception != null)
                {
                    logger?.LogWarning(
                        "Retrying users API invocation due to exception '{ResultExceptionType}:{ResultExceptionMessage}', delaying for {RetryDelay}ms, then attempting retry {RetryAttempt}",
                        result.Exception.GetType().Name,
                        result.Exception.Message,
                        timeSpan.TotalMilliseconds,
                        retryCount);
                    return;
                }

                logger?.LogWarning(
                    "Retrying users API invocation due to response status '{ResultResponseStatusCode}', delaying for {RetryDelay}ms, then attempting retry {RetryAttempt}",
                    result.Result.StatusCode,
                    timeSpan.TotalMilliseconds,
                    retryCount);
            };
    }


    private static IServiceCollection AddServicesConfigOptions(this IServiceCollection services)
    {
        services.AddOptions<ServicesConfigOptions>().Configure<IConfiguration>((options, conf) => conf.BindRequiredOptions(options));
        return services;
    }
}
