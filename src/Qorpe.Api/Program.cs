using AutoMapper;
using Qorpe.Api;
using Qorpe.Application;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;
using Qorpe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapReverseProxy();

// await LoadConfigs(app.Services);

//app.Map("/update", context =>
//{
//    context.RequestServices.GetRequiredService<ProxyConfigProvider>().Update(GetRoutes(), GetClusters());
//    return Task.CompletedTask;
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
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

    ClusterConfig[] clusterConfigs = [.. await clusterRepository.LoadAsync()];
    RouteConfig[] routeConfigs = [.. await routeRepository.LoadAsync()];

    Yarp.ReverseProxy.Configuration.ClusterConfig[] mappedClusterConfigs
        = mapper.Map<Yarp.ReverseProxy.Configuration.ClusterConfig[]>(clusterConfigs);

    Yarp.ReverseProxy.Configuration.RouteConfig[] mappedRouteConfigs
        = mapper.Map<Yarp.ReverseProxy.Configuration.RouteConfig[]>(routeConfigs);

    // Update in-memory configuration
    var inMemoryConfigProvider = services.GetRequiredService<IInMemoryConfigProvider>();
    inMemoryConfigProvider.Update(mappedRouteConfigs, mappedClusterConfigs);
}
