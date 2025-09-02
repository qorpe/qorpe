using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Application.Features.Tenants;
using Qorpe.Hub.Contracts.V1.Tenants.Models;

namespace Qorpe.Hub.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("hub/v{version:apiVersion}/tenants")]
public class TenantsController(ITenantsService svc, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("{key}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<TenantInfo>> GetByKey([FromRoute] string key, CancellationToken ct)
        => (await svc.GetByKeyAsync(key, ct))
            .Adapt<TenantInfo>();

    [HttpGet("by-domain/{domain}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<TenantInfo>> GetByDomain([FromRoute] string domain, CancellationToken ct)
        => (await svc.GetByDomainAsync(domain, ct))
            .Adapt<TenantInfo>();
    
    /// <summary>
    /// Lists memberships of the authenticated user.
    /// </summary>
    [HttpGet("mine")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<TenantInfo>>> GetMine(CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrEmpty(currentUser.UserId))
            return Unauthorized();

        var items = (await svc.GetMineAsync(currentUser.UserId, ct))
            .Adapt<IReadOnlyList<TenantInfo>>();
        return Ok(items);
    }
}