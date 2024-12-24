using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Addresses;

namespace PhotoSiTest.Addresses.Persistence.Configurations;

public class AddressConfiguration : EntityConfigurationBase<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);

        builder.ToTable("Addresses");

        builder.Property(a => a.Location).HasMaxLength(AddressConstants.LocationMaxLength).IsRequired();
    }
}
