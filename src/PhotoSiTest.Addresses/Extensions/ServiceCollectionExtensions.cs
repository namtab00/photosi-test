using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSiTest.Addresses.Domain.Mapping;
using PhotoSiTest.Addresses.Persistence;
using PhotoSiTest.Addresses.Services;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Contracts.Domain.Addresses;

namespace PhotoSiTest.Addresses.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAddressesService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AddressMappingProfile));
        services.AddPostgresDbContext<AddressesDbContext>(configuration, AddressesDbContext.MigrationHistoryTableName);
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressService, AddressService>();
        return services;
    }
}
