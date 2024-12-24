using PhotoSiTest.Common.Data;
using PhotoSiTest.Common.Exceptions;

namespace PhotoSiTest.Orders.Domain;

public class Order : PhotoSiTestEntity
{
    public Guid DeliveryAddressId { get; set; }

    public List<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount { get; private set; }

    public Guid UserId { get; set; }


    public void AddItem(OrderItem item)
    {
        Items.Add(item);
        RefreshTotal();
    }


    public void RefreshTotal()
    {
        TotalAmount = Items.Sum(i => i.UnitPrice * i.Quantity);
    }


    public void RemoveItem(Guid itemId)
    {
        var orderItem = Items.FirstOrDefault(i => i.Id == itemId) ?? throw new EntityNotFoundException<OrderItem>(itemId);
        Items.Remove(orderItem);
        RefreshTotal();
    }
}
