using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Common.Configurations;

public sealed class InMemoryConfig : IProxyConfig
{
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
