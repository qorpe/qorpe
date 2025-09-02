namespace Qorpe.Hub.Application.Features.Auth.Models;

/** Request DTO to rotate refresh token. */
public sealed record RefreshRequest(string RefreshToken);