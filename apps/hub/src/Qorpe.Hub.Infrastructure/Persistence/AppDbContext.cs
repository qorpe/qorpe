using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Qorpe.BuildingBlocks.Extensions;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IAppDbContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<UserTenant> UserTenants => Set<UserTenant>();
    
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        
        b.HasDefaultSchema("hub");
        
        foreach (var entity in b.Model.GetEntityTypes())
        {
            // Tables
            entity.SetTableName(entity.GetTableName()?.ToSnakeCase().RemovePrefix("asp_net_").Singularize(false));

            // Columns
            foreach (var property in entity.GetProperties())
            {
                var storeObject = StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema());
                var columnName = property.GetColumnName(storeObject) ?? property.Name;
                property.SetColumnName(columnName.ToSnakeCase());
            }

            // Primary / Alternate keys
            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ToSnakeCase());
            }

            // Foreign Keys
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName()?.ToSnakeCase());
            }

            // Indexes
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
            }
        }
        
        b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}