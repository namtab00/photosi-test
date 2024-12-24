using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Users.Domain.Mapping;
using PhotoSiTest.Users.Persistence;
using PhotoSiTest.Users.Services;

namespace PhotoSiTest.Users.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUsersService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddPostgresDbContext<UsersDbContext>(configuration, UsersDbContext.MigrationHistoryTableName);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
