using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Orders.Domain;

namespace PhotoSiTest.Orders.Persistence.Configurations;

public sealed class OrderConfiguration : EntityConfigurationBase<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Orders");
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2).IsRequired();
    }
}
