using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Contracts.Domain.Orders;
using PhotoSiTest.Orders.Domain.Mapping;
using PhotoSiTest.Orders.Persistence;
using PhotoSiTest.Orders.Services;

namespace PhotoSiTest.Orders.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrdersService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(OrderMappingProfile));
        services.AddPostgresDbContext<OrdersDbContext>(configuration, OrdersDbContext.MigrationHistoryTableName);
        services.AddScoped<IOrderRepository, OrdersRepository>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}
