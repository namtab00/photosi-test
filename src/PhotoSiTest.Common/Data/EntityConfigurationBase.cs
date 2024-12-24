using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhotoSiTest.Common.Data;

public abstract class EntityConfigurationBase<T> : IEntityTypeConfiguration<T>
    where T : PhotoSiTestEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
