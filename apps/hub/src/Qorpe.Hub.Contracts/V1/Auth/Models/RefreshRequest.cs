namespace Qorpe.Hub.Contracts.V1.Auth.Models;

/** Request DTO to rotate refresh token. */
public sealed record RefreshRequest(string RefreshToken);