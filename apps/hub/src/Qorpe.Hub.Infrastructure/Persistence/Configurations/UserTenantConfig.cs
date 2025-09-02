using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Infrastructure.Persistence.Configurations;

public class UserTenantConfig : IEntityTypeConfiguration<UserTenant>
{
    public void Configure(EntityTypeBuilder<UserTenant> b)
    {
        b.HasKey(x => new { x.UserId, x.TenantId });

        b.Property(x => x.Role).HasMaxLength(64);
        b.Property(x => x.IsActive).HasDefaultValue(true);
        b.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now() at time zone 'utc'");

        b.HasIndex(x => x.TenantId);
        b.HasIndex(x => x.UserId);
        b.HasIndex(x => new { x.TenantId, x.Role });

        b.HasOne(x => x.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); // prefer Cascade

        b.HasOne(x => x.Tenant)
            .WithMany(t => t.Memberships)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade); // prefer Cascade
    }
}