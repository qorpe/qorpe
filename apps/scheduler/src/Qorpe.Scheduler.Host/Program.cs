using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Qorpe.BuildingBlocks.Auth;
using Qorpe.BuildingBlocks.Multitenancy;
using Qorpe.Hub.SDK.Extensions;
using Qorpe.Scheduler.Application.Features.Calendars;
using Qorpe.Scheduler.Application.Features.Jobs;
using Qorpe.Scheduler.Application.Features.Scheduler;
using Qorpe.Scheduler.Application.Features.Triggers;
using Qorpe.Scheduler.Host.Common.Handlers;
using Qorpe.Scheduler.Infrastructure.Auth;
using Quartz;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("TenantMatch", policy =>
        policy.Requirements.Add(new TenantMatchRequirement()));

builder.Services.AddSingleton<IAuthorizationHandler, TenantMatchHandler>();
builder.Services.AddScoped<IJobsService, JobsService>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();
builder.Services.AddScoped<ITriggersService, TriggersService>();
builder.Services.AddScoped<ICalendarsService, CalendarsService>();

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

#region Quartz

builder.Services.AddQuartz(q =>
{
    q.SchedulerName = "QuartzScheduler";
    q.SchedulerId = "AUTO";

    q.UsePersistentStore(options =>
    {
        options.PerformSchemaValidation = true;
        options.UseProperties = true;
        options.RetryInterval = TimeSpan.FromSeconds(15);
        options.UsePostgres(ops =>
        {
            ops.ConnectionString = builder.Configuration.GetConnectionString("qorpe") ?? string.Empty;
            ops.TablePrefix = "scheduler.qrtz_";
        });

        options.UseClustering(c =>
        {
            c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
            c.CheckinInterval = TimeSpan.FromSeconds(10);
        });
        options.UseSystemTextJsonSerializer();
    });

    q.UseDefaultThreadPool(threadPoolOptions =>
    {
        threadPoolOptions.MaxConcurrency = 5;
    });
    
    q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 5);

    // // 1) Job'u kaydet (durable = triggersız da store'da tutulur)
    // q.AddJob<HeartbeatJob>(opts => opts
    //     .WithIdentity("heartbeat", "system")
    //     .StoreDurably());
    //
    // // 2A) Basit tetikleyici: 5 sn’de bir, hemen başla
    // q.AddTrigger(opts => opts
    //     .ForJob("heartbeat", "system")
    //     .WithIdentity("heartbeat-every-5s", "system")
    //     .StartNow()
    //     .WithSimpleSchedule(s => s
    //         .WithInterval(TimeSpan.FromSeconds(5))
    //         .RepeatForever()
    //         .WithMisfireHandlingInstructionNowWithExistingCount()));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.AwaitApplicationStarted = true;
});


#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
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