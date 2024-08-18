using System.Security.Authentication;

namespace Qorpe.Domain.Entities;

public sealed class HttpClientConfig
{
    public long Id { get; set; }

    // public static readonly HttpClientConfig Empty = new();

    public SslProtocols? SslProtocols { get; init; }

    public bool? DangerousAcceptAnyServerCertificate { get; init; }

    public int? MaxConnectionsPerServer { get; init; }

    public WebProxyConfig? WebProxy { get; init; }

    public bool? EnableMultipleHttp2Connections { get; init; }

    public string? RequestHeaderEncoding { get; init; }

    public string? ResponseHeaderEncoding { get; init; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
