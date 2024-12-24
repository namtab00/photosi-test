using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Products;
using PhotoSiTest.Products.Domain;

namespace PhotoSiTest.Products.Persistence.Configurations;

public sealed class ProductCategoryConfiguration : EntityConfigurationBase<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);
        builder.ToTable("ProductCategories");

        builder.Property(c => c.Name).HasMaxLength(ProductCategoryConstants.NameMaxLength).IsRequired();

        builder.Property(c => c.Description).HasMaxLength(ProductCategoryConstants.DescriptionMaxLength);

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
