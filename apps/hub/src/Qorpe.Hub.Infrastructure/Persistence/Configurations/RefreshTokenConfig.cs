using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        // Define the primary key
        b.HasKey(e => e.Id).HasName("refresh_token_pkey");
        
        b.HasIndex(x => new { x.TenantId, x.UserId, x.TokenHash }).IsUnique();
        b.HasIndex(x => x.ExpiresAtUtc);
        
        // Configure properties with column mappings and constraints
        b.Property(e => e.TenantId).IsRequired();
        b.Property(e => e.UserId).HasMaxLength(36).IsRequired();
        b.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
        b.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
        b.Property(e => e.ExpiresAtUtc);
        b.Property(e => e.CreatedAtUtc).HasDefaultValueSql("now() at time zone 'utc'");
        b.Property(e => e.RevokedAtUtc);
        b.Property(e => e.DeviceInfo).HasMaxLength(256);
        b.Property(e => e.CreatedByIp).HasMaxLength(45);
        
        // Relation: RefreshToken.TenantId -> Tenants.Id
        b.HasOne<Tenant>()
            .WithMany(t => t.RefreshTokens)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Relation: RefreshToken.UserId -> ApplicationUser.Id
        b.HasOne<ApplicationUser>()
            .WithMany(t => t.RefreshTokens)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}