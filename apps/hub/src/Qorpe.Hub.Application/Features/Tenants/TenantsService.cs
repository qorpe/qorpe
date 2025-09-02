using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Application.Features.Tenants.Models;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Application.Features.Tenants;

/// <summary>Loads tenants from DB and caches them in-memory.</summary>
public class TenantsService(IAppDbContext db, IMemoryCache cache) : ITenantsService
{
    /** Gets tenant by key and caches for 5 minutes. */
    public async Task<TenantInfo?> GetByKeyAsync(string key, CancellationToken ct)
    {
        var cacheKey = $"tenant:key:{key}".ToLowerInvariant();
        if (cache.TryGetValue(cacheKey, out TenantInfo? dto)) return dto;

        dto = await db.Tenants.AsNoTracking()
            .Where(t => t.Key == key && t.IsActive)
            .Select(t => new TenantInfo(t.Id, t.Key, t.Name, t.Domain, t.IsActive))
            .FirstOrDefaultAsync(ct);

        if (dto is not null)
            cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
                { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), SlidingExpiration = TimeSpan.FromMinutes(2) });

        return dto;
    }

    /** Optional: domain lookup. */
    public async Task<TenantInfo?> GetByDomainAsync(string domain, CancellationToken ct)
    {
        var cacheKey = $"tenant:domain:{domain}".ToLowerInvariant();
        if (cache.TryGetValue(cacheKey, out TenantInfo? dto)) return dto;

        dto = await db.Tenants.AsNoTracking()
            .Where(t => t.Domain == domain && t.IsActive)
            .Select(t => new TenantInfo(t.Id, t.Key, t.Name, t.Domain, t.IsActive))
            .FirstOrDefaultAsync(ct);

        if (dto is not null)
            cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
                { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), SlidingExpiration = TimeSpan.FromMinutes(2) });

        return dto;
    }

    public async Task<IReadOnlyList<TenantInfo>> GetMineAsync(string userId, CancellationToken ct)
    {
        var q = db.UserTenants.AsNoTracking()
            .Join(db.Tenants.AsNoTracking(), ut => ut.TenantId, t => t.Id, (ut, t) => new { ut, t })
            .Where(t1 => t1.ut.UserId == userId && t1.ut.IsActive && t1.t.IsActive)
            .OrderBy(t1 => t1.t.Name)
            .Select(t1 => new TenantInfo(t1.t.Id, t1.t.Key, t1.t.Name, t1.t.Domain, t1.t.IsActive));

        return await q.ToListAsync(ct);
    }
}