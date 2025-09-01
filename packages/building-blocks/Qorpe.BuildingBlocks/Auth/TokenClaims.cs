namespace Qorpe.BuildingBlocks.Auth;

/** Well-known claim names to avoid magic strings. */
public abstract class TokenClaims
{
    public const string Subject = "sub";
    public const string TenantId = "tid";
    public const string TenantKey = "tkey";
    public const string Username = "unique_name";
    public const string Email = "email";
}