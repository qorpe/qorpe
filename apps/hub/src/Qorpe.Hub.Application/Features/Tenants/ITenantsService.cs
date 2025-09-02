using Qorpe.Hub.Application.Features.Tenants.Models;

namespace Qorpe.Hub.Application.Features.Tenants;

public interface ITenantsService
{
    Task<TenantInfo?> GetByKeyAsync(string key, CancellationToken ct);
    Task<TenantInfo?> GetByDomainAsync(string domain, CancellationToken ct);
    Task<IReadOnlyList<TenantInfo>> GetMineAsync(string userId, CancellationToken ct);
}