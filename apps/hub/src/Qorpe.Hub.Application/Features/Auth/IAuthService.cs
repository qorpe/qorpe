using Qorpe.Hub.Contracts.V1.Auth.Models;

namespace Qorpe.Hub.Application.Features.Auth;

/** Auth application service contract. */
public interface IAuthService
{
    Task<TokenResponse> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<TokenResponse> LoginAsync(LoginRequest req, CancellationToken ct);
    Task<TokenResponse> RefreshAsync(RefreshRequest req, CancellationToken ct);
    Task LogoutAsync(string refreshToken, CancellationToken ct);
}