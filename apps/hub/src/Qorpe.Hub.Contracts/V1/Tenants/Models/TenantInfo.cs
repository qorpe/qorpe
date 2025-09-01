namespace Qorpe.Hub.Contracts.V1.Tenants.Models;

public sealed record TenantInfo(
    long Id, 
    string Key, 
    string Name, 
    string? Domain, 
    bool IsActive);