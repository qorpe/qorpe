using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Common.Configurations;

public sealed class InMemoryConfigProvider
{
    private volatile InMemoryConfig _config;

    public InMemoryConfigProvider(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        : this(routes, clusters, Guid.NewGuid().ToString())
    { }

    public InMemoryConfigProvider(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId)
    {
        _config = new InMemoryConfig(routes, clusters, revisionId);
    }

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
}
