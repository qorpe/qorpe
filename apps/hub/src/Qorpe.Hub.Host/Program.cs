using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qorpe.BuildingBlocks.Auth;
using Qorpe.BuildingBlocks.Multitenancy;
using Qorpe.Hub.Application.Common.Interfaces;
using Qorpe.Hub.Application.Features.Auth;
using Qorpe.Hub.Application.Features.Tenants;
using Qorpe.Hub.Domain.Entities;
using Qorpe.Hub.Host.Common.Handlers;
using Qorpe.Hub.Infrastructure.Auth;
using Qorpe.Hub.Infrastructure.Persistence;
using Qorpe.Hub.SDK.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Aspire: DbContext
builder.AddNpgsqlDbContext<AppDbContext>(
    "qorpe", 
    settings => {},
    options =>
    {
        // options.UseNpgsql(o =>
        //     o.MigrationsHistoryTable("__ef_migrations_history", "hub")); // 👈 name + schema
    }
);

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
    {
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequiredLength = 8;
        o.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Auth services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<ITenantsService, TenantsService>();

builder.Services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<AppDbContext>());

builder.Services.AddHubTenantsClient(new Uri(builder.Configuration["Services:Gate:BaseUrl"]!)); // Refit client
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITenantAccessor, TenantAccessor>();
builder.Services.AddScoped<ITenantSetter>(sp => (ITenantSetter)sp.GetRequiredService<ITenantAccessor>());

// Authentication for consuming protected endpoints (if needed)
builder.Services.AddJwtAuth(builder.Configuration.GetSection("JwtOptions"), o =>
{
    o.Events = new JwtBearerEvents
    {
        OnTokenValidated = _ => Task.CompletedTask
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TenantMatch", policy =>
        policy.Requirements.Add(new TenantMatchRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, TenantMatchHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

#region Api Versioning

// API Versioning
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;

    o.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version"));
})
.AddMvc()
.AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    // 🔴 Database Migration
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var cs = db.Database.GetConnectionString();
        app.Logger.LogInformation("Applying migrations on {Conn}", cs);

        await db.Database.MigrateAsync();
        app.Logger.LogInformation("Migrations applied.");
    }
    catch (Exception ex)
    {
        app.Logger.LogCritical(ex, "DB migrate failed");
        throw;
    }
}

app.UseHttpsRedirection();

app.Use(async (ctx, next) =>
{
    if (!ctx.Request.RouteValues.TryGetValue("tenant", out var tv) || tv is null)
    {
        await next(ctx);
        return;
    }

    var tenantKey = tv.ToString();
    if (!string.IsNullOrWhiteSpace(tenantKey))
    {
        long.TryParse(ctx.User.FindFirst(TokenClaims.TenantId)?.Value, out var tid);
        var setter = ctx.RequestServices.GetRequiredService<ITenantSetter>();
        setter.TrySet(new TenantContext(tid, tenantKey));
    }

    await next(ctx);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 

app.Run();