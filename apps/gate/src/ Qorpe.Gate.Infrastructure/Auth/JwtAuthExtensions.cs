using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qorpe.BuildingBlocks.Auth;

namespace Qorpe.Gate.Infrastructure.Auth;

/** Registers JwtBearer using shared JwtOptions. */
public static class JwtAuthExtensions
{
    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfigurationSection jwtSection,
        Action<JwtBearerOptions>? configureBearer = null)
    {
        services.Configure<JwtOptions>(jwtSection);
        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        // Read once for validation parameters
        var tmp = new JwtOptions();
        jwtSection.Bind(tmp);

        var keyBytes = string.IsNullOrWhiteSpace(tmp.Base64Key) ? null : Convert.FromBase64String(tmp.Base64Key);
        var securityKey = keyBytes is null ? null : new SymmetricSecurityKey(keyBytes);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, ValidIssuer = tmp.Issuer,
                        ValidateAudience = true, ValidAudience = tmp.Audience,
                        ValidateIssuerSigningKey = securityKey is not null,
                        IssuerSigningKey = securityKey,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };

                    // Extra configuration hook (e.g., OnTokenValidated tenant check)
                    configureBearer?.Invoke(o);
                });

        return services;
    }

    /** Simple options validation to fail fast on misconfigured. */
    private sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
    {
        public ValidateOptionsResult Validate(string? name, JwtOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Issuer)) return ValidateOptionsResult.Fail("Jwt:Issuer is required.");
            if (string.IsNullOrWhiteSpace(options.Audience)) return ValidateOptionsResult.Fail("Jwt:Audience is required.");
            // HS256 path: Base64Key required; RS256 path: would need different options (e.g., PublicKey)
            return string.IsNullOrWhiteSpace(options.Base64Key) ? ValidateOptionsResult.Fail("Jwt:Base64Key is required for HS256.") : ValidateOptionsResult.Success;
        }
    }
}