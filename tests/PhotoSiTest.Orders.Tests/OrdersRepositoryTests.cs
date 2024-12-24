using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Contracts.Domain.Orders;
using PhotoSiTest.Orders.Domain;
using PhotoSiTest.Orders.Persistence;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Orders.Tests;

public class OrdersRepositoryTests : RepositoryTestBase<OrdersDbContext>
{
    public OrdersRepositoryTests()
    {
        _repository = new OrdersRepository(Context);
    }


    protected override OrdersDbContext CreateContext() => new(Options);


    private readonly OrdersRepository _repository;


    [Fact]
    public async Task AddAsync_ShouldAddOrderWithItems()
    {
        // Arrange
        var order = new Order {
            UserId = Guid.NewGuid(),
            DeliveryAddressId = Guid.NewGuid(),
            Items = [new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 10.00m }]
        };

        // Act
        var result = await _repository.AddAsync(order);

        // Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Items.Count.ShouldBe(1);
        result.UserId.ShouldBe(order.UserId);

        // Verify persistence
        var dbOrder = await Context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == result.Id);
        dbOrder.ShouldNotBeNull();
        dbOrder.Items.Count.ShouldBe(1);
    }


    [Fact]
    public async Task DeleteAsync_ShouldRemoveOrderAndItems()
    {
        // Arrange
        var order = new Order {
            UserId = Guid.NewGuid(),
            DeliveryAddressId = Guid.NewGuid(),
            Items = [new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 10.00m }]
        };

        await _repository.AddAsync(order);

        // Act
        await _repository.DeleteAsync(order.Id);

        // Assert
        var deletedOrder = await Context.Orders.FindAsync(order.Id);
        deletedOrder.ShouldBeNull();

        var orderItems = await Context.OrderItems.Where(i => i.OrderId == order.Id).ToListAsync();
        orderItems.ShouldBeEmpty();
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrderWithItems()
    {
        // Arrange
        var order = new Order {
            UserId = Guid.NewGuid(),
            DeliveryAddressId = Guid.NewGuid(),
            Items = [new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 10.00m }]
        };

        await _repository.AddAsync(order);

        // Clear context to ensure fresh load
        Context.ChangeTracker.Clear();

        // Act
        var result = await _repository.GetByIdAsync(order.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(order.Id);
        result.Items.Count.ShouldBe(1);
    }
}
