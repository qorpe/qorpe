using Qorpe.Hub.Contracts.V1.Tenants.Models;
using Refit;

namespace Qorpe.Hub.SDK.Clients;

public interface ITenantsClient
{
    [Get("/hub/v{version}/tenants/{key}")]
    Task<TenantInfo?> GetByKeyAsync(
        [AliasAs("version")] string version, 
        [AliasAs("key")] string key, CancellationToken ct = default);

    [Get("/hub/v{version}/tenants/by-domain/{domain}")]
    Task<TenantInfo?> GetByDomainAsync(
        [AliasAs("version")] string version, 
        [AliasAs("domain")] string domain, CancellationToken ct = default);
}