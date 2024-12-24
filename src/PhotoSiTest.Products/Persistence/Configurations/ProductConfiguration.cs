using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Products.Domain;

namespace PhotoSiTest.Products.Persistence.Configurations;

public sealed class ProductConfiguration : EntityConfigurationBase<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
        builder.ToTable("Products");

        builder.Property(p => p.Name).HasMaxLength(ProductConstants.NameMaxLength).IsRequired();

        builder.Property(p => p.Description).HasMaxLength(ProductConstants.DescriptionMaxLength);

        builder.Property(p => p.UnitPrice).HasPrecision(18, 2).IsRequired();

        builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Name);
    }
}
