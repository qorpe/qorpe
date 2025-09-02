using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Infrastructure.Persistence.Configurations;

/** EF Core mapping for Tenant (PostgreSQL). */
public class TenantConfig : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> b)
    {
        // Define the primary key
        b.HasKey(e => e.Id).HasName("tenant_pkey");
        
        // Unique constraints
        b.HasIndex(x => x.Key).IsUnique();

        // In PostgreSQL, UNIQUE allows multiple NULLs; ensure uniqueness only when NOT NULL.
        b.HasIndex(x => x.Domain).HasFilter("\"domain\" is not null").IsUnique();

        // Configure properties with column mappings and constraints
        b.Property(x => x.Key).HasMaxLength(64).IsRequired();
        b.Property(x => x.Name).HasMaxLength(128).IsRequired();
        b.Property(x => x.Domain).HasMaxLength(255);
        b.Property(x => x.Metadata).HasColumnType("jsonb");
        b.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now() at time zone 'utc'");
    }
}