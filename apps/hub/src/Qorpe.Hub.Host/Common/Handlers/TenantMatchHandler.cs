using Microsoft.AspNetCore.Authorization;
using Qorpe.BuildingBlocks.Auth;

namespace Qorpe.Hub.Host.Common.Handlers;

public sealed class TenantMatchHandler : AuthorizationHandler<TenantMatchRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext ctx, TenantMatchRequirement req)
    {
        var http = (ctx.Resource as DefaultHttpContext) ?? (ctx.Resource as HttpContext);
        if (http is null) return Task.CompletedTask;

        var routeTenant = http.Request.RouteValues.TryGetValue("tenant", out var tVal) ? tVal?.ToString() : null;

        var tid  = ctx.User.FindFirst(TokenClaims.TenantId)?.Value;   // GUID
        var tkey = ctx.User.FindFirst(TokenClaims.TenantKey)?.Value;  // string key

        var ok = false;
        if (!string.IsNullOrWhiteSpace(routeTenant) && !string.IsNullOrWhiteSpace(tkey))
            ok |= string.Equals(routeTenant, tkey, StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrWhiteSpace(routeTenant) && Guid.TryParse(routeTenant, out var rGuid) &&
            Guid.TryParse(tid, out var tidGuid))
            ok |= rGuid == tidGuid;

        if (ok) ctx.Succeed(req); else ctx.Fail();
        return Task.CompletedTask;
    }
}

public sealed class TenantMatchRequirement : IAuthorizationRequirement
{
}