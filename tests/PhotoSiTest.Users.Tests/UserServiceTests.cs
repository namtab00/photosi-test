using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using PhotoSiTest.Tests.Common;
using PhotoSiTest.Users.Domain;
using PhotoSiTest.Users.Domain.Mapping;
using PhotoSiTest.Users.Persistence;
using PhotoSiTest.Users.Services;
using Shouldly;

namespace PhotoSiTest.Users.Tests;

public class UserServiceTests : ServiceTestBase
{
    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository, Mapper);
    }


    private readonly UserService _userService;

    private readonly IUserRepository _userRepository;


    protected override void ConfigureMapper(IMapperConfigurationExpression config)
    {
        config.AddProfile<UserMappingProfile>();
    }


    [Fact]
    public void AutoMapper_ProfileConfiguration_IsValid()
    {
        // Assert
        MapperConfiguration.AssertConfigurationIsValid();
    }


    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldThrow()
    {
        var existingUser = new User { Email = "test@test.com" };
        var createDto = new CreateUserDto("test@test.com", "Test", "User");

        _userRepository.GetListAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(new List<User> { existingUser });

        await Should.ThrowAsync<InvalidOperationException>(() => _userService.CreateUserAsync(createDto));
    }


    [Fact]
    public async Task CreateUser_WithUniqueEmail_ShouldSucceed()
    {
        var createDto = new CreateUserDto("test@test.com", "Test", "User");

        _userRepository.GetListAsync().Returns(new List<User>());
        _userRepository.AddAsync(Arg.Any<User>()).Returns(x => x.Arg<User>());

        var result = await _userService.CreateUserAsync(createDto);

        result.ShouldNotBeNull();
        result.Email.ShouldBe(createDto.Email);
    }
}
