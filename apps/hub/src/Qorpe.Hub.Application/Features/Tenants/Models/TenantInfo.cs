namespace Qorpe.Hub.Application.Features.Tenants.Models;

public sealed record TenantInfo(
    long Id, 
    string Key, 
    string Name, 
    string? Domain, 
    bool IsActive);