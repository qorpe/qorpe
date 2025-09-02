namespace Qorpe.Hub.Application.Features.Auth.Models;

/** Request DTO to register a new user under current tenant. */
public sealed record RegisterRequest(string Email, string Password, string? DisplayName);