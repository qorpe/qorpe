namespace Qorpe.BuildingBlocks.Auth;

/** Common JWT options used across services. */
public class JwtOptions
{
    // e.g. "qorpe.auth"
    public string Issuer { get; set; } = null!;

    // e.g. "qorpe.clients"
    public string Audience { get; set; } = null!;

    // Base64-encoded 256-bit secret for HS256 (or empty if RS256 is used)
    public string Base64Key { get; set; } = string.Empty;

    // Access token lifetime (minutes)
    public int AccessTokenMinutes { get; set; } = 15;

    // Refresh token lifetime (days)
    public int RefreshTokenDays { get; set; } = 30;
}