using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Orders.Domain;

namespace PhotoSiTest.Orders.Persistence.Configurations;

public sealed class OrderItemConfiguration : EntityConfigurationBase<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrderItems");

        builder.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();

        builder.Property(i => i.Quantity).IsRequired();

        builder.HasOne(i => i.Order).WithMany(o => o.Items).HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
    }
}
