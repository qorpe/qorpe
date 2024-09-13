using AutoMapper;
using Qorpe.Api;
using Qorpe.Application;
using Qorpe.Application.Common.Configurations;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;
using Qorpe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

var clusterRepository = app.Services.GetRequiredService<IClusterRepository>();
var routeRepository = app.Services.GetRequiredService<IRouteRepository>();
var mapper = app.Services.GetRequiredService<IMapper>();
    
ClusterConfig[] clusterConfigs = [.. clusterRepository.AsQueryable()];
RouteConfig[] routeConfigs = [.. routeRepository.AsQueryable()];
    
Yarp.ReverseProxy.Configuration.ClusterConfig[] mappedClusterConfigs 
    = mapper.Map<Yarp.ReverseProxy.Configuration.ClusterConfig[]>(clusterConfigs);
    
Yarp.ReverseProxy.Configuration.RouteConfig[] mappedRouteConfigs 
    = mapper.Map<Yarp.ReverseProxy.Configuration.RouteConfig[]>(routeConfigs);
    
app.Services.GetRequiredService<InMemoryConfigProvider>().Update(mappedRouteConfigs, mappedClusterConfigs);

app.MapReverseProxy();

//app.Map("/update", context =>
//{
//    context.RequestServices.GetRequiredService<InMemoryConfigProvider>().Update(GetRoutes(), GetClusters());
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
