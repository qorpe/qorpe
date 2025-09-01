using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qorpe.BuildingBlocks.Auth;
using Qorpe.BuildingBlocks.Multitenancy;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Application.Features.Tenants;
using Qorpe.Hub.Contracts.V1.Auth.Models;
using Qorpe.Hub.Contracts.V1.Tenants.Models;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Application.Features.Auth;

/** Auth service using ASP.NET Identity + refresh token rotation. */
public class AuthService(
    ITenantsService tenantsClient,
    ITenantSetter tenantSetter,
    UserManager<ApplicationUser> users,
    IAppDbContext db,
    ITenantAccessor tenantAccessor,
    ITokenService tokens,
    Microsoft.Extensions.Options.IOptions<JwtOptions> opt)
    : IAuthService
{
    private readonly JwtOptions _opt = opt.Value;

    public async Task<TokenResponse> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var t = tenantAccessor.Current ?? throw new InvalidOperationException("Tenant not resolved.");
        var tenant = await tenantsClient.GetByKeyAsync(t.Key, ct)
                     ?? throw new InvalidOperationException("Tenant not found.");
        tenantSetter.TryEnrich(tenant.Id, tenant.Key);
        
        var user = new ApplicationUser
        {
            TenantId = t.Id,
            UserName = req.Email,
            Email = req.Email,
            DisplayName = req.DisplayName,
            EmailConfirmed = false,
            IsActive = true
        };
        var result = await users.CreateAsync(user, req.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(";", result.Errors.Select(e => e.Description)));

        return await IssueTokensAsync(user, req.Email, tenant, device: "register", ip: null, ct);
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest req, CancellationToken ct)
    {
        var t = tenantAccessor.Current ?? throw new InvalidOperationException("Tenant not resolved.");
        var tenant = await tenantsClient.GetByKeyAsync(t.Key, ct)
                     ?? throw new InvalidOperationException("Tenant not found.");
        tenantSetter.TryEnrich(tenant.Id, tenant.Key);
        var user = await FindUserByUsernameOrEmailAsync(tenant.Id, req.UsernameOrEmail, ct)
                   ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!user.IsActive) throw new UnauthorizedAccessException("User inactive.");
        if (!await users.CheckPasswordAsync(user, req.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return await IssueTokensAsync(user, user.Email!, tenant, req.DeviceInfo, ip: null, ct);
    }

    public async Task<TokenResponse> RefreshAsync(RefreshRequest req, CancellationToken ct)
    {
        var t = tenantAccessor.Current ?? throw new InvalidOperationException("Tenant not resolved.");
        var tenant = await tenantsClient.GetByKeyAsync(t.Key, ct)
                     ?? throw new InvalidOperationException("Tenant not found.");
        tenantSetter.TryEnrich(tenant.Id, tenant.Key);
        var hash = tokens.HashRefreshToken(req.RefreshToken);

        var entity = await db.RefreshTokens.AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && x.TokenHash == hash && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow)
            .FirstOrDefaultAsync(ct);

        if (entity is null) throw new UnauthorizedAccessException("Refresh token invalid.");

        var user = await users.FindByIdAsync(entity.UserId)
                   ?? throw new UnauthorizedAccessException("User not found.");

        // revoke old + issue new (rotation)
        await RevokeAsync(hash, ct);
        return await IssueTokensAsync(user, user.Email!, tenant, device: "refresh", ip: null, ct);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct)
    {
        var t = tenantAccessor.Current ?? throw new InvalidOperationException("Tenant not resolved.");
        var hash = tokens.HashRefreshToken(refreshToken);
        await RevokeAsync(hash, ct);
    }

    #region Helper(s)

    private async Task<TokenResponse> IssueTokensAsync(
        ApplicationUser user, string email, TenantInfo tenant, string? device, string? ip, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var claims = new List<System.Security.Claims.Claim>
        {
            new(TokenClaims.Subject, user.Id),
            new(TokenClaims.TenantId, tenant.Id.ToString()),
            new(TokenClaims.TenantKey, tenant.Key),
            new(TokenClaims.Username, user.UserName ?? email),
            new(TokenClaims.Email, email)
        };

        var access = tokens.CreateAccessToken(claims, now, out var exp);
        var refresh = tokens.CreateRefreshToken();
        var r = new RefreshToken
        {
            TenantId = tenant.Id,
            UserId = user.Id,
            TokenHash = tokens.HashRefreshToken(refresh),
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(_opt.RefreshTokenDays),
            DeviceInfo = device,
            CreatedByIp = ip
        };
        db.RefreshTokens.Add(r);
        await db.SaveChangesAsync(ct);

        return new TokenResponse(access, refresh, exp, user.Id, user.UserName ?? email, tenant.Key);
    }

    private async Task RevokeAsync(string hash, CancellationToken ct)
    {
        var token = await db.RefreshTokens
            .Where(x => x.TokenHash == hash && x.RevokedAtUtc == null)
            .FirstOrDefaultAsync(ct);

        if (token is not null)
        {
            token.RevokedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }
    }

    private async Task<ApplicationUser?> FindUserByUsernameOrEmailAsync(long? tenantId, string input, CancellationToken ct)
    {
        var norm = users.NormalizeName(input);
        var normEmail = users.NormalizeEmail(input);

        return await users.Users.AsNoTracking()
            .Where(u => u.TenantId == tenantId &&
                        (u.NormalizedUserName == norm || u.NormalizedEmail == normEmail))
            .FirstOrDefaultAsync(ct);
    }

    #endregion
}