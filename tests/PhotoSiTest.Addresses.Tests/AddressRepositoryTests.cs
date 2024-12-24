using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Addresses.Persistence;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Addresses.Tests;

public class AddressRepositoryTests : RepositoryTestBase<AddressesDbContext>
{
    public AddressRepositoryTests()
    {
        _repository = new AddressRepository(Context);
    }


    protected override AddressesDbContext CreateContext() => new(Options);


    private readonly AddressRepository _repository;


    [Fact]
    public async Task AddAsync_ShouldAddAddress()
    {
        // Arrange
        var address = new Address {
            UserId = Guid.NewGuid(),
            Location = "Test location"
        };

        // Act
        var result = await _repository.AddAsync(address);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);

        var dbAddress = await Context.Addresses.FindAsync(result.Id);
        dbAddress.ShouldNotBeNull();
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAddresses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var addresses = new[] {
            new Address { UserId = userId, Location = "via Tal dei Tali 1, Roma" },
            new Address { UserId = userId, Location = "via Tal dei Tali 2, Roma" }
        };

        await Context.Addresses.AddRangeAsync(addresses);
        await Context.SaveChangesAsync();

        // Act
        var results = await _repository.GetListAsync();

        // Assert
        results.ShouldNotBeNull();
        results.Count().ShouldBe(2);
    }
}
