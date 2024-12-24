using PhotoSiTest.Common.Mapping;
using PhotoSiTest.Contracts.Domain.Orders.Dtos;

namespace PhotoSiTest.Orders.Domain.Mapping;

public class OrderMappingProfile : MappingProfileBase
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<CreateOrderDto, Order>()
            .ForMember(d => d.TotalAmount, opt => opt.Ignore())
            .ForMember(d => d.Items, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.OrderId, opt => opt.Ignore())
            .ForMember(d => d.UnitPrice, opt => opt.Ignore())
            .ForMember(d => d.Order, opt => opt.Ignore());

        CreateMap<UpdateOrderDto, Order>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Items, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore())
            .ForMember(d => d.TotalAmount, opt => opt.Ignore())
            .ForMember(d => d.DeliveryAddressId, opt => opt.Ignore());
    }
}
