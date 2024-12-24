using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Users.Domain;

namespace PhotoSiTest.Users.Persistence.Configurations;

public sealed class UserConfiguration : EntityConfigurationBase<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");
        builder.Property(u => u.Email).HasMaxLength(UserConstants.EmailMaxLength).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(UserConstants.FirstNameMaxLength).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(UserConstants.LastNameMaxLength).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
