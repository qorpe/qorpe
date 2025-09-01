using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Contracts.V1.Tenants.Models;

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

    public async Task<TenantInfo?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        var cacheKey = $"tenant:username:{username}".ToLowerInvariant();
        if (cache.TryGetValue(cacheKey, out TenantInfo? dto)) return dto;

        dto = await db.Tenants.AsNoTracking()
            .Where(t => t.Users != null && t.Users.Select(u => u.UserName).Contains(username) && t.IsActive)
            .Select(t => new TenantInfo(t.Id, t.Key, t.Name, t.Domain, t.IsActive))
            .FirstOrDefaultAsync(ct);

        if (dto is not null)
            cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
                { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), SlidingExpiration = TimeSpan.FromMinutes(2) });

        return dto;
    }
}