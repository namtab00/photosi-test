using Microsoft.Extensions.Options;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Common.Extensions;
using PhotoSiTest.Contracts.Domain.Addresses;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;
using PhotoSiTest.Contracts.Domain.Orders;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;

namespace PhotoSiTest.API.HostedServices;

public class SampleDataSeeder(IServiceProvider serviceProvider, ILogger<SampleDataSeeder> logger, IOptions<DataSeedingOptions> options) : IDataSeeder
{
    private readonly DataSeedingOptions _options = options.Value;

    private readonly List<(Guid Id, string Name, decimal Price)> _products = [];

    private readonly List<(Guid Id, string Email)> _users = [];


    public async Task SeedAsync(CancellationToken ct)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var addressService = serviceProvider.GetRequiredService<IAddressService>();
            var categoryService = serviceProvider.GetRequiredService<IProductCategoryService>();
            var productService = serviceProvider.GetRequiredService<IProductService>();
            var orderService = serviceProvider.GetRequiredService<IOrderService>();
            var userService = serviceProvider.GetRequiredService<IUserService>();

            await SeedUsers(userService, ct);
            await SeedAddresses(addressService, ct);
            await SeedProducts(categoryService, productService, ct);
            await SeedOrders(addressService, orderService, ct);

            logger.LogInformation("Data seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }


    private async Task SeedAddresses(IAddressService addressService, CancellationToken ct)
    {
        foreach (var (userId, _) in _users)
        {
            for (var i = 0; i < _options.AddressesPerUser; i++)
            {
                var address = new CreateAddressDto(userId, $"via Tal dei Tali {i + 100}, Roma, RM, CAP 1000{i + 1}, Italia");
                var result = await addressService.CreateAddressAsync(address, ct);
                logger.LogInformation("Seeded address {AddressId} for user: {UserId}", result.Id, userId);
            }
        }
    }


    private async Task SeedOrders(IAddressService addressService, IOrderService orderService, CancellationToken ct)
    {
        var random = new Random();

        foreach (var (userId, email) in _users)
        {
            var userAddresses = await addressService.GetUserAddressesAsync(userId, ct);

            foreach (var address in userAddresses.Where(a => a.UserId == userId))
            {
                var orderCount = random.Next(1, _options.MaxOrdersPerAddress + 1);
                for (var i = 0; i < orderCount; i++)
                {
                    var addOrderItemDtos = _products.OrderBy(_ => random.Next())
                        .Take(random.Next(1, _options.MaxProductsPerOrder + 1))
                        .Select(p => new AddOrderItemDto(p.Id, random.Next(1, 5)))
                        .ToList();

                    var order = new CreateOrderDto(userId, address.Id);
                    var orderDto = await orderService.CreateOrderAsync(order, ct);

                    foreach (var addOrderItemDto in addOrderItemDtos)
                    {
                        await orderService.AddOrderItemAsync(orderDto.Id, addOrderItemDto, ct);
                    }

                    logger.LogInformation("Seeded order {OrderId} for user: {Email}", orderDto.Id, email);
                }
            }
        }
    }


    private async Task SeedProducts(IProductCategoryService categoryService, IProductService productService, CancellationToken ct)
    {
        var random = new Random();

        for (var i = 0; i < _options.CategoriesCount; i++)
        {
            var categoryToSeed = new CreateProductCategoryDto($"Category {i + 1}", $"Description for category {i + 1}");

            Guid productCategoryId;
            var existingCategory = await categoryService.GetCategoryByNameAsync(categoryToSeed.Name, ct);
            if (existingCategory != null)
            {
                logger.LogInformation("Skipping seed of existing category with name {CategoryName}", existingCategory.Name);
                productCategoryId = existingCategory.Id;
            }
            else
            {
                var categoryResult = await categoryService.CreateCategoryAsync(categoryToSeed, ct);
                logger.LogInformation("Seeded category: {Name}", categoryToSeed.Name);
                productCategoryId = categoryResult.Id;
            }


            for (var j = 0; j < _options.ProductsPerCategory; j++)
            {
                var price = random.NextDecimal(_options.MinProductPrice, _options.MaxProductPrice, 2);

                var product = new CreateProductDto($"Product {i}-{j}", $"Description for product {i}-{j}", price, productCategoryId);

                var productResult = await productService.CreateProductAsync(product, ct);
                _products.Add((productResult.Id, product.Name, product.UnitPrice));
                logger.LogInformation("Seeded product: {Name}", product.Name);
            }
        }
    }


    private async Task SeedUsers(IUserService userService, CancellationToken ct)
    {
        for (var i = 0; i < _options.UsersCount; i++)
        {
            var user = new CreateUserDto($"user{i}@example.com", $"FirstName{i}", $"LastName{i}");

            var existingUser = await userService.GetUserByEmailAsync(user.Email, ct);
            if (existingUser != null)
            {
                logger.LogInformation("Skipping seed of existing user with email {Email}", user.Email);
                continue;
            }

            var result = await userService.CreateUserAsync(user, ct);
            _users.Add((result.Id, user.Email));
            logger.LogInformation("Seeded user: {Email}", user.Email);
        }
    }
}
