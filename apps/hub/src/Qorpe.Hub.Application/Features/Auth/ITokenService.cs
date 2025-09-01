using System.Security.Claims;

namespace Qorpe.Hub.Application.Features.Auth;

/** Issues JWT access tokens and random refresh tokens. */
public interface ITokenService
{
    string CreateAccessToken(IEnumerable<Claim> claims, DateTime nowUtc, out DateTime expiresAtUtc);
    string CreateRefreshToken();
    string HashRefreshToken(string token); // SHA256
}