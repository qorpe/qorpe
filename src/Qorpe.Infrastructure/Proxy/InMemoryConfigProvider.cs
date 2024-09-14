using AutoMapper;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Proxy;

public class InMemoryConfigProvider(
    IMapper mapper, IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId) 
    : IProxyConfigProvider
{
    private volatile InMemoryConfig _config = new(routes, clusters, revisionId);

    public InMemoryConfigProvider(
        IMapper mapper, IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        : this(mapper, routes, clusters, Guid.NewGuid().ToString())
    { }

    public IProxyConfig GetConfig() => _config;

    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var newConfig = new InMemoryConfig(routes, clusters);
        UpdateInternal(newConfig);
    }

    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId)
    {
        var newConfig = new InMemoryConfig(routes, clusters, revisionId);
        UpdateInternal(newConfig);
    }

    private void UpdateInternal(InMemoryConfig newConfig)
    {
        var oldConfig = Interlocked.Exchange(ref _config, newConfig);
        oldConfig.SignalChange();
    }

    public void UpdateRoute(string routeId, RouteConfig newRouteConfig)
    {
        var currentRoutes = _config.Routes.ToList();
        var routeIndex = currentRoutes.FindIndex(r => r.RouteId == routeId);

        if (routeIndex >= 0)
        {
            // Replace the route with the new configuration
            currentRoutes[routeIndex] = newRouteConfig;

            // Update the configuration with the new settings
            Update(currentRoutes, _config.Clusters);
        }
    }

    public void UpdateRoute(string routeId, Action<Qorpe_Entities.RouteConfig> updateAction)
    {
        // Convert current routes to a list
        var currentRoutes = _config.Routes.ToList();

        // Find the index of the route to be updated
        var routeIndex = currentRoutes.FindIndex(r => r.RouteId == routeId);

        if (routeIndex >= 0)
        {
            // Get the old route config
            var oldRouteConfig = currentRoutes[routeIndex];

            // Convert the old route config to a mutable form
            var mutableRouteConfig = mapper.Map<Qorpe_Entities.RouteConfig>(oldRouteConfig);

            // Apply the update action
            updateAction(mutableRouteConfig);

            // Convert the updated mutable route config back to immutable form
            var updatedRouteConfig = mapper.Map<RouteConfig>(mutableRouteConfig);

            // Replace the old route config with the updated one
            currentRoutes[routeIndex] = updatedRouteConfig;

            // Update the configuration with the new settings
            Update(currentRoutes, _config.Clusters);
        }
    }

    public void UpdateCluster(string clusterId, ClusterConfig newClusterConfig)
    {
        var currentClusters = _config.Clusters.ToList();
        var clusterIndex = currentClusters.FindIndex(c => c.ClusterId == clusterId);

        if (clusterIndex >= 0)
        {
            // Replace the cluster with the new configuration
            currentClusters[clusterIndex] = newClusterConfig;

            // Update the configuration with the new settings
            Update(_config.Routes, currentClusters);
        }
    }

    public void UpdateCluster(string clusterId, Action<Qorpe_Entities.ClusterConfig> updateAction)
    {
        // Convert current clusters to a list
        var currentClusters = _config.Clusters.ToList();

        // Find the index of the cluster to be updated
        var clusterIndex = currentClusters.FindIndex(c => c.ClusterId == clusterId);

        if (clusterIndex >= 0)
        {
            // Get the old cluster config
            var oldClusterConfig = currentClusters[clusterIndex];

            // Convert the old cluster config to a mutable form
            var mutableClusterConfig = mapper.Map<Qorpe_Entities.ClusterConfig>(oldClusterConfig);

            // Apply the update action
            updateAction(mutableClusterConfig);

            // Convert the updated mutable cluster config back to immutable form
            var updatedClusterConfig = mapper.Map<ClusterConfig>(mutableClusterConfig);

            // Replace the old cluster config with the updated one
            currentClusters[clusterIndex] = updatedClusterConfig;

            // Update the configuration with the new settings
            Update(_config.Routes, currentClusters);
        }
    }

    private class InMemoryConfig : IProxyConfig
    {
        // Used to implement the change token for the state
        private readonly CancellationTokenSource _cts = new();

        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
            : this(routes, clusters, Guid.NewGuid().ToString())
        { }

        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId)
        {
            RevisionId = revisionId ?? throw new ArgumentNullException(nameof(revisionId));
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        public string RevisionId { get; }

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken { get; }

        internal void SignalChange()
        {
            _cts.Cancel();
        }
    }
}
