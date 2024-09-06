using System.Security.Authentication;

namespace Qorpe.Domain.Entities.Lite;

public sealed class HttpClientConfig
{
    public SslProtocols? SslProtocols { get; set; }

    public bool? DangerousAcceptAnyServerCertificate { get; set; }

    public int? MaxConnectionsPerServer { get; set; }

    public WebProxyConfig? WebProxy { get; set; }

    public bool? EnableMultipleHttp2Connections { get; set; }

    public string? RequestHeaderEncoding { get; set; }

    public string? ResponseHeaderEncoding { get; set; }
}
