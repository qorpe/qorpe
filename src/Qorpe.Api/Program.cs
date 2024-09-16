using AutoMapper;
using Qorpe.Api;
using Qorpe.Application;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe_Entities = Qorpe.Domain.Entities;
using Qorpe.Infrastructure;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapReverseProxy();

await LoadConfigs(app.Services);

//app.Map("/update", context =>
//{
//    context.RequestServices.GetRequiredService<ProxyConfigProvider>().Update(GetRoutes(), GetClusters());
//    return Task.CompletedTask;
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Load initial configuration from the database on startup
static async Task LoadConfigs(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var routeRepository = scope.ServiceProvider.GetRequiredService<IRouteRepository>();
    var clusterRepository = scope.ServiceProvider.GetRequiredService<IClusterRepository>();
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

    Qorpe_Entities.ClusterConfig[] clusterConfigs = [.. await clusterRepository.LoadAsync()];
    Qorpe_Entities.RouteConfig[] routeConfigs = [.. await routeRepository.LoadAsync()];

    ClusterConfig[] mappedClusterConfigs = mapper.Map<ClusterConfig[]>(clusterConfigs);

    RouteConfig[] mappedRouteConfigs = mapper.Map<RouteConfig[]>(routeConfigs);

    // Update in-memory configuration
    var inMemoryConfigProvider = services.GetRequiredService<InMemoryConfigProvider>();
    inMemoryConfigProvider.Update(mappedRouteConfigs, mappedClusterConfigs);
}
