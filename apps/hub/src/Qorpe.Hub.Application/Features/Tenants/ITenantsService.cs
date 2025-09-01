using Qorpe.Hub.Contracts.V1.Tenants.Models;

namespace Qorpe.Hub.Application.Features.Tenants;

public interface ITenantsService
{
    Task<TenantInfo?> GetByKeyAsync(string key, CancellationToken ct);
    Task<TenantInfo?> GetByDomainAsync(string domain, CancellationToken ct);
    Task<TenantInfo?> GetByUsernameAsync(string username, CancellationToken ct);
}