using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Orders.Domain;

public class OrderItem : PhotoSiTestEntity
{
    public Order Order { get; set; } = null!;

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
