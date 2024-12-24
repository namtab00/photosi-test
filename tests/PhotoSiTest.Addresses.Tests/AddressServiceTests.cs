using AutoMapper;
using NSubstitute;
using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Addresses.Domain.Mapping;
using PhotoSiTest.Addresses.Persistence;
using PhotoSiTest.Addresses.Services;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Addresses.Tests;

public class AddressServiceTests : ServiceTestBase
{
    public AddressServiceTests()
    {
        _addressRepository = Substitute.For<IAddressRepository>();
        _userService = Substitute.For<IUserService>();
        _addressService = new AddressService(_addressRepository, _userService, Mapper);
    }


    private readonly AddressService _addressService;

    private readonly IAddressRepository _addressRepository;

    private readonly IUserService _userService;


    protected override void ConfigureMapper(IMapperConfigurationExpression config)
    {
        config.AddProfile<AddressMappingProfile>();
    }


    [Fact]
    public void AutoMapper_ProfileConfiguration_IsValid()
    {
        // Assert
        MapperConfiguration.AssertConfigurationIsValid();
    }


    [Fact]
    public async Task CreateAddressAsync_WithInvalidUser_ShouldThrow()
    {
        // Arrange
        var createDto = new CreateAddressDto(Guid.NewGuid(), "via Tal dei Tali 1, Roma");

        _userService.FindUserAsync(Arg.Any<Guid>()).Returns((UserDto?)null);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidEntityReferenceException>(async () => await _addressService.CreateAddressAsync(createDto));
        exception.EntityTypeName.ShouldBe("User");
        exception.EntityId.ShouldBe(createDto.UserId);
    }


    [Fact]
    public async Task CreateAddressAsync_WithValidUser_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createDto = new CreateAddressDto(userId, "via Tal dei Tali 1, Roma");

        _userService.FindUserAsync(userId, Arg.Any<CancellationToken>()).Returns(new UserDto(userId, "test@test.com", "Test", "User"));

        _addressRepository.GetListAsync().Returns(new List<Address>());
        _addressRepository.AddAsync(Arg.Any<Address>()).Returns(x => x.Arg<Address>());

        // Act
        var result = await _addressService.CreateAddressAsync(createDto);

        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(userId);
    }
}
