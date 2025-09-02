namespace Qorpe.Hub.Application.Features.Auth.Models;

public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAtUtc,
    string UserId,
    string Username,
    string TenantKey);