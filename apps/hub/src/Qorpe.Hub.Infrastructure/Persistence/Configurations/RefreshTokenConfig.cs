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
        b.Property(e => e.UserId).IsRequired();
        b.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
        b.Property(e => e.ExpiresAtUtc);
        b.Property(e => e.CreatedAtUtc).HasDefaultValueSql("now() at time zone 'utc'");
        b.Property(e => e.RevokedAtUtc);
        b.Property(e => e.DeviceInfo).HasMaxLength(256);
        b.Property(e => e.CreatedByIp).HasMaxLength(45);
        
        // (RefreshToken -> Tenant)
        b.HasOne(rt => rt.Tenant)
            .WithMany(t => t.RefreshTokens)
            .HasForeignKey(rt => rt.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // (RefreshToken -> ApplicationUser)
        b.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}