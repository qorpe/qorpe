using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Application.Features.Auth;
using Qorpe.Hub.Contracts.V1.Auth.Models;
using App = Qorpe.Hub.Application.Features.Auth.Models;

namespace Qorpe.Hub.Host.Controllers.V1;

/// <summary>Authentication endpoints (tenant-scoped via a path /t/{key}).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/hub/v{version:apiVersion}/auth")]
public class AuthController(IAuthService svc, ICurrentUser currentUser) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<TokenResponse> Register(string tenant, [FromBody] RegisterRequest req, CancellationToken ct)
        => (await svc.RegisterAsync(req.Adapt<App.RegisterRequest>(), ct))
            .Adapt<TokenResponse>();

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<TokenResponse> Login(string tenant, LoginRequest req, CancellationToken ct)
        => (await svc.LoginAsync(req.Adapt<App.LoginRequest>(), ct))
            .Adapt<TokenResponse>();


    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<TokenResponse> Refresh(string tenant, [FromBody] RefreshRequest req, CancellationToken ct)
        => (await svc.RefreshAsync(req.Adapt<App.RefreshRequest>(), ct))
            .Adapt<TokenResponse>();

    [HttpPost("logout")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<IActionResult> Logout(string tenant, RefreshRequest req, CancellationToken ct)
    {
        await svc.LogoutAsync(req.RefreshToken, ct);
        return NoContent();
    }

    [HttpPost("auth/exchange")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<ActionResult<TokenResponse>> Exchange(
        string tenant, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrEmpty(currentUser.UserId))
            return Unauthorized();

        var response = (await svc.ExchangeAsync(currentUser.UserId, tenant, ct))
            .Adapt<IReadOnlyList<TokenResponse>>();
        return Ok(response);
    }
}