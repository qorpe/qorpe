using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qorpe.BuildingBlocks.Auth;

namespace Qorpe.Hub.Application.Features.Auth;

/** Issues JWT HS256 tokens and secure refresh tokens. */
public class TokenService : ITokenService
{
    private readonly JwtOptions _opt;
    private readonly SigningCredentials _credentials;

    public TokenService(IOptions<JwtOptions> opt)
    {
        _opt = opt.Value;
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_opt.Base64Key));
        _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public string CreateAccessToken(IEnumerable<Claim> claims, DateTime nowUtc, out DateTime expiresAtUtc)
    {
        expiresAtUtc = nowUtc.AddMinutes(_opt.AccessTokenMinutes);
        var jwt = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: nowUtc,
            expires: expiresAtUtc,
            signingCredentials: _credentials);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string CreateRefreshToken()
    {
        Span<byte> bytes = stackalloc byte[32]; // 256-bit
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    public string HashRefreshToken(string token)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Convert.FromBase64String(token));
        return Convert.ToHexString(hash); // 64 hex chars
    }
}