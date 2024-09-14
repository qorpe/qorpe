using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Common.Interfaces;

public interface IInMemoryConfigProvider : IProxyConfigProvider
{
    void AddCluster(ClusterConfig newClusterConfig);
    void AddRoute(RouteConfig newRouteConfig);
    void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters);
    void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId);
    void UpdateRoute(string routeId, RouteConfig newRouteConfig);
    void UpdateRoute(string routeId, Action<Qorpe_Entities.RouteConfig> updateAction);
    void UpdateCluster(string clusterId, ClusterConfig newClusterConfig);
    void UpdateCluster(string clusterId, Action<Qorpe_Entities.ClusterConfig> updateAction);
    void RemoveCluster(string clusterId);
    void RemoveRoute(string routeId);
}
