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
public class AuthController(IAuthService svc, ICurrentUser currentUser, IHttpContextAccessor accessor) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<TokenResponse> Register(string tenant, [FromBody] RegisterRequest req, CancellationToken ct)
        => (await svc.RegisterAsync(req.Adapt<App.RegisterRequest>(), ct))
            .Adapt<TokenResponse>();

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<TokenResponse> Login(string tenant, [FromBody] LoginRequest req, CancellationToken ct)
    {
        var result = (await svc.LoginAsync(req.Adapt<App.LoginRequest>(), ct)).Adapt<TokenResponse>();

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromDays(14),
            IsEssential = true
        });

        return result;
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<TokenResponse> Refresh(string tenant, [FromBody] RefreshRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.RefreshToken))
        {
            var cookieToken = accessor.HttpContext?.Request.Cookies["refresh_token"];
            if (string.IsNullOrWhiteSpace(cookieToken))
                throw new UnauthorizedAccessException("Refresh token missing.");
            req = new RefreshRequest(RefreshToken: cookieToken);
        }

        var result = (await svc.RefreshAsync(req.Adapt<App.RefreshRequest>(), ct)).Adapt<TokenResponse>();

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromDays(14),
            IsEssential = true
        });

        return result;
    }

    [HttpPost("logout")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<IActionResult> Logout(string tenant, [FromBody] RefreshRequest req, CancellationToken ct)
    {
        var token = string.IsNullOrWhiteSpace(req.RefreshToken)
            ? (accessor.HttpContext?.Request.Cookies["refresh_token"] ?? string.Empty)
            : req.RefreshToken;

        if (!string.IsNullOrWhiteSpace(token))
            await svc.LogoutAsync(token, ct);

        Response.Cookies.Delete("refresh_token", new CookieOptions
        {
            Path = "/",
            SameSite = SameSiteMode.Strict,
            Secure = true
        });

        return NoContent();
    }

    [HttpPost("exchange")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<ActionResult<TokenResponse>> Exchange(string tenant, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrEmpty(currentUser.UserId))
            return Unauthorized();

        var response = await svc.ExchangeAsync(currentUser.UserId, tenant, ct);
        return Ok(response.Adapt<TokenResponse>());
    }
}