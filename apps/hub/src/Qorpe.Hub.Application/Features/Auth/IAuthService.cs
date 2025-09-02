using Qorpe.Hub.Application.Features.Auth.Models;

namespace Qorpe.Hub.Application.Features.Auth;

/** Auth application service contract. */
public interface IAuthService
{
    Task<TokenResponse> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<TokenResponse> LoginAsync(LoginRequest req, CancellationToken ct);
    Task<TokenResponse> RefreshAsync(RefreshRequest req, CancellationToken ct);
    Task LogoutAsync(string refreshToken, CancellationToken ct);
    Task<TokenResponse> ExchangeAsync(string userId, string tenantKey, CancellationToken ct);
}