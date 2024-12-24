using AutoMapper;
using NSubstitute;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Addresses;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Contracts.Domain.Products.Dtos;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using PhotoSiTest.Orders.Domain;
using PhotoSiTest.Orders.Domain.Mapping;
using PhotoSiTest.Orders.Persistence;
using PhotoSiTest.Orders.Services;
using PhotoSiTest.Tests.Common;
using Shouldly;

namespace PhotoSiTest.Orders.Tests;

public class OrderServiceTests : ServiceTestBase
{
    public OrderServiceTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _userService = Substitute.For<IUserService>();
        _addressService = Substitute.For<IAddressService>();
        _productService = Substitute.For<IProductService>();
        _orderService = new OrderService(_orderRepository, Mapper, _userService, _addressService, _productService);
    }


    protected override void ConfigureMapper(IMapperConfigurationExpression config)
    {
        config.AddProfile<OrderMappingProfile>();
    }


    private readonly OrderService _orderService;

    private readonly IOrderRepository _orderRepository;

    private readonly IUserService _userService;

    private readonly IAddressService _addressService;

    private readonly IProductService _productService;


    [Fact]
    public async Task AddOrderItem_WithInvalidOrder_ShouldThrow()
    {
        // Arrange
        _orderRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Order?)null);

        var dto = new AddOrderItemDto(Guid.NewGuid(), 1);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException<Order>>(async () => await _orderService.AddOrderItemAsync(Guid.NewGuid(), dto));
    }


    [Fact]
    public async Task AddOrderItem_WithInvalidProduct_ShouldThrow()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        _orderRepository.GetByIdAsync(orderId).Returns(new Order { Id = orderId });
        _productService.FindProductAsync(productId).Returns((ProductDto?)null);

        var dto = new AddOrderItemDto(productId, 1);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidEntityReferenceException>(async () => await _orderService.AddOrderItemAsync(orderId, dto));

        exception.EntityTypeName.ShouldBe("Product");
        exception.EntityId.ShouldBe(productId);
    }


    [Fact]
    public async Task AddOrderItem_WithValidData_ShouldAddItemAndUpdateTotal()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var existingOrder = new Order {
            Id = orderId,
            Items = [new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 10.00m }]
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _productService.FindProductAsync(productId).Returns(new ProductDto(productId, "Test Product", "Description", 20.00m, Guid.NewGuid()));

        var dto = new AddOrderItemDto(productId, 2);

        // Act
        var result = await _orderService.AddOrderItemAsync(orderId, dto);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(2);
        result.TotalAmount.ShouldBe(50.00m); // 1*10 + 2*20
        await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>());
    }


    [Fact]
    public void AutoMapper_ProfileConfiguration_IsValid()
    {
        // Assert
        MapperConfiguration.AssertConfigurationIsValid();
    }


    [Fact]
    public async Task CreateOrder_WithInvalidAddress_ShouldThrow()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createDto = new CreateOrderDto(UserId: userId, DeliveryAddressId: Guid.NewGuid());

        _userService.FindUserAsync(userId).Returns(new UserDto(userId, "test@test.com", "Test", "User"));

        _addressService.FindAddressAsync(createDto.DeliveryAddressId).Returns((AddressDto?)null);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidEntityReferenceException>(async () => await _orderService.CreateOrderAsync(createDto));

        exception.EntityTypeName.ShouldBe("Address");
        exception.EntityId.ShouldBe(createDto.DeliveryAddressId);
    }


    [Fact]
    public async Task CreateOrder_WithInvalidUser_ShouldThrow()
    {
        // Arrange
        var createDto = new CreateOrderDto(UserId: Guid.NewGuid(), DeliveryAddressId: Guid.NewGuid());

        _userService.FindUserAsync(createDto.UserId).Returns((UserDto?)null);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidEntityReferenceException>(async () => await _orderService.CreateOrderAsync(createDto));

        exception.EntityTypeName.ShouldBe("User");
        exception.EntityId.ShouldBe(createDto.UserId);
    }


    [Fact]
    public async Task CreateOrder_WithValidReferences_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        var createDto = new CreateOrderDto(UserId: userId, DeliveryAddressId: addressId);

        _userService.FindUserAsync(userId).Returns(new UserDto(userId, "test@test.com", "Test", "User"));

        _addressService.FindAddressAsync(addressId).Returns(new AddressDto(addressId, userId, "via Tal dei Tali 1, Roma"));

        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(x => x.Arg<Order>());

        // Act
        var result = await _orderService.CreateOrderAsync(createDto);

        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(userId);
        result.DeliveryAddressId.ShouldBe(addressId);
        result.TotalAmount.ShouldBe(0m);
    }


    [Fact]
    public async Task RemoveOrderItem_WithInvalidOrder_ShouldThrow()
    {
        // Arrange
        _orderRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Order?)null);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException<Order>>(async () => await _orderService.RemoveOrderItemAsync(Guid.NewGuid(), Guid.NewGuid()));
    }


    [Fact]
    public async Task RemoveOrderItem_WithInvalidOrderItem_ShouldThrow()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var existingOrder = new Order {
            Id = orderId,
            Items = [new OrderItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 10.00m }]
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException<OrderItem>>(async () => await _orderService.RemoveOrderItemAsync(orderId, Guid.NewGuid()));
    }


    [Fact]
    public async Task RemoveOrderItem_WithValidData_ShouldRemoveItemAndUpdateTotal()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderItemId = Guid.NewGuid();
        var existingOrder = new Order {
            Id = orderId,
            Items = [
                new OrderItem { Id = orderItemId, ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 10.00m },
                new OrderItem { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 20.00m }
            ]
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);

        // Act
        var result = await _orderService.RemoveOrderItemAsync(orderId, orderItemId);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.TotalAmount.ShouldBe(20.00m);
        await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>());
    }
}
