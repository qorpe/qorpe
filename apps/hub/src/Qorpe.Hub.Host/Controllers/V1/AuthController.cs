using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Hub.Application.Features.Auth;
using Qorpe.Hub.Contracts.V1.Auth.Models;

namespace Qorpe.Hub.Host.Controllers.V1;

/// <summary>Authentication endpoints (tenant-scoped via a path /t/{key}).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/hub/v{version:apiVersion}/auth")]
public class AuthController(IAuthService svc) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public Task<TokenResponse> Register(string tenant, [FromBody] RegisterRequest req, CancellationToken ct)
        => svc.RegisterAsync(req, ct);

    [HttpPost("login")]
    [AllowAnonymous]
    public Task<TokenResponse> Login(string tenant, [FromBody] LoginRequest req, CancellationToken ct)
        => svc.LoginAsync(req, ct);

    [HttpPost("refresh")]
    [AllowAnonymous]
    public Task<TokenResponse> Refresh(string tenant, [FromBody] RefreshRequest req, CancellationToken ct)
        => svc.RefreshAsync(req, ct);

    [HttpPost("logout")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<IActionResult> Logout(string tenant, [FromBody] RefreshRequest req, CancellationToken ct)
    {
        await svc.LogoutAsync(req.RefreshToken, ct);
        return NoContent();
    }
}