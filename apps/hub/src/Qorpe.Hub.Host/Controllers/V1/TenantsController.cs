using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Hub.Application.Features.Tenants;
using Qorpe.Hub.Contracts.V1.Tenants.Models;

namespace Qorpe.Hub.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("hub/v{version:apiVersion}/tenants")]
public class TenantsController(ITenantsService tenantsService) : ControllerBase
{
    [HttpGet("{key}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<TenantInfo>> GetByKey([FromRoute] string key, CancellationToken ct)
    {
        var dto = await tenantsService.GetByKeyAsync(key, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("by-domain/{domain}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<TenantInfo>> GetByDomain([FromRoute] string domain, CancellationToken ct)
    {
        var dto = await tenantsService.GetByDomainAsync(domain, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
    
    [HttpGet("by-username/{username}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<TenantInfo>> GetByUsername([FromRoute] string username, CancellationToken ct)
    {
        var dto = await tenantsService.GetByUsernameAsync(username, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}