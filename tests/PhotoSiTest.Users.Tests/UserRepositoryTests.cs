using PhotoSiTest.Tests.Common;
using PhotoSiTest.Users.Domain;
using PhotoSiTest.Users.Persistence;
using Shouldly;

namespace PhotoSiTest.Users.Tests;

public class UserRepositoryTests : RepositoryTestBase<UsersDbContext>
{
    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context);
    }


    protected override UsersDbContext CreateContext() => new(Options);


    private readonly UserRepository _repository;


    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        var user = new User {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var result = await _repository.AddAsync(user);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Email.ShouldBe(user.Email);

        var dbUser = await Context.Users.FindAsync(result.Id);
        dbUser.ShouldNotBeNull();
        dbUser.Email.ShouldBe(user.Email);
    }


    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        // Arrange
        var user = new User {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user.Id);

        // Assert
        var deletedUser = await Context.Users.FindAsync(user.Id);
        deletedUser.ShouldBeNull();
    }


    [Fact]
    public async Task DeleteAsync_WithNonExistingUser_ShouldNotThrow()
    {
        // Act & Assert
        await Should.NotThrowAsync(async () => await _repository.DeleteAsync(Guid.NewGuid()));
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new[] {
            new User { Email = "test1@test.com", FirstName = "Test1", LastName = "User1" },
            new User { Email = "test2@test.com", FirstName = "Test2", LastName = "User2" }
        };
        await Context.Users.AddRangeAsync(users);
        await Context.SaveChangesAsync();

        // Act
        var results = await _repository.GetListAsync();

        // Assert
        results.ShouldNotBeNull();
        results.Count().ShouldBe(2);
    }


    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var user = new User {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Email.ShouldBe(user.Email);
        result.FirstName.ShouldBe(user.FirstName);
    }


    [Fact]
    public async Task GetByIdAsync_WithNonExistingUser_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.ShouldBeNull();
    }


    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        var user = new User {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        user.FirstName = "Updated";
        user.LastName = "Name";
        await _repository.UpdateAsync(user);

        // Assert
        var updatedUser = await Context.Users.FindAsync(user.Id);
        updatedUser.ShouldNotBeNull();
        updatedUser.FirstName.ShouldBe("Updated");
        updatedUser.LastName.ShouldBe("Name");
    }
}
