namespace Qorpe.Hub.Contracts.V1.Auth.Models;

/** Response DTO containing access and refresh tokens. */
public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAtUtc,
    string UserId,
    string Username,
    string TenantKey);