using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> b)
    {
        b.HasIndex(x => x.NormalizedUserName).IsUnique();
        b.HasIndex(x => x.NormalizedEmail);

        b.Property(x => x.DisplayName).HasMaxLength(128);
        b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
    }
}