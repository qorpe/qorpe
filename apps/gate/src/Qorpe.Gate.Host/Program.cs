using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Qorpe.BuildingBlocks.Auth;
using Qorpe.BuildingBlocks.Multitenancy;
using Qorpe.Gate.Host.Common.Handlers;
using Qorpe.Gate.Infrastructure.Auth;
using Qorpe.Hub.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

// YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

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

var allowedHosts = builder.Configuration["AllowedHosts"] ?? "*";
var allowedOrigins = allowedHosts.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (allowedOrigins is ["*"])
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders();
        }
    });
});

builder.Services.ConfigureApplicationCookie(o =>
{
    o.Cookie.SameSite = SameSiteMode.None;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("TenantMatch", p => p.Requirements.Add(new TenantMatchRequirement()));

builder.Services.AddSingleton<IAuthorizationHandler, TenantMatchHandler>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // Production: Serve Vite build output from wwwroot
    // Copy UI dist folder into wwwroot (e.g., /ui/dist → /wwwroot)
    app.UseDefaultFiles();   // for index.html
    app.UseStaticFiles();    // for js/css/assets

    // For SPA routes: if not an API request and 404, fallback to index.html
    app.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == 404 &&
            !context.Request.Path.StartsWithSegments("/t"))
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "index.html"));
        }
    });
    
    // app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/console"), consoleApp =>
    // {
    //     var fs = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "console-ui", "dist"));
    //
    //     // Static assets served under the /console path
    //     consoleApp.UseStaticFiles(new StaticFileOptions {
    //         FileProvider = fs,
    //         RequestPath = "/console"
    //     });
    //
    //     // SPA fallback: /console/* -> index.html
    //     consoleApp.Run(async ctx =>
    //     {
    //         ctx.Response.ContentType = "text/html; charset=UTF-8";
    //         await ctx.Response.SendFileAsync(fs.GetFileInfo("index.html"));
    //     });
    // });
}

app.UseHttpsRedirection();

app.UseForwardedHeaders();

app.UseCors("CorsPolicy");

// Gate Branch (works only under /gate)
// only for /t/{tenant}/gate/v{version}/...
app.MapWhen(
    IsTenantGatePath, 
    branch =>
    {
        branch.Use(async (ctx, next) =>
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
        branch.UseRouting();
        branch.UseAuthentication();
        branch.UseAuthorization();
        branch.UseEndpoints(e => e.MapControllers().RequireAuthorization("TenantMatch"));
    });

app.MapReverseProxy();

app.Run();
return;

static bool IsTenantGatePath(HttpContext ctx)
{
    var path = ctx.Request.Path.Value ?? "";
    return Regex.IsMatch(path, @"^/t/[^/]+/gate/v[^/]+(?:/|$)", RegexOptions.IgnoreCase);
}