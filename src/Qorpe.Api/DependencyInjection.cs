using Qorpe.Api.Handlers;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Api;

public static class DependencyInjection
{
    const string DEBUG_METADATA_KEY = "debug";
    const string DEBUG_VALUE = "true";

    public static IServiceCollection AddApiServices(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        services.AddReverseProxy()
                .LoadFromMemory(GetRoutes(), GetClusters());

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.ToString());
            options.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }

    private static RouteConfig[] GetRoutes()
    {
        return
        [
            //new RouteConfig()
            //{
            //    RouteId = "route" + Random.Shared.Next(), // Forces a new route id each time GetRoutes is called.
            //    ClusterId = "cluster2",
            //    Match = new RouteMatch
            //    {
            //        // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
            //        Path = "{**catch-all}"
            //    }
            //}
        ];
    }

    private static ClusterConfig[] GetClusters()
    {
        var debugMetadata = new Dictionary<string, string>
        {
            { DEBUG_METADATA_KEY, DEBUG_VALUE }
        };

        return
        [
            //new ClusterConfig()
            //{
            //    ClusterId = "cluster2",
            //    // SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
            //    Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            //    {
            //        // { "destination1", new DestinationConfig() { Address = "https://example.com" } },
            //        { "destination1", new DestinationConfig() {
            //            Address = "https://jsonplaceholder.typicode.com/"}
            //        }
            //    }
            //}
        ];
    }
}
