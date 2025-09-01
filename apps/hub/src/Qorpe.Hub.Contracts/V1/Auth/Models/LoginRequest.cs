namespace Qorpe.Hub.Contracts.V1.Auth.Models;

/** Request DTO to authenticate user with password. */
public sealed record LoginRequest(string UsernameOrEmail, string Password, string? DeviceInfo);