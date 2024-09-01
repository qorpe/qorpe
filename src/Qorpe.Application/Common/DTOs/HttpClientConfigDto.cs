using System.Security.Authentication;

namespace Qorpe.Application.Common.DTOs;

public sealed class HttpClientConfigDto
{
    public SslProtocols? SslProtocols { get; set; }
    public bool? DangerousAcceptAnyServerCertificate { get; set; }
    public int? MaxConnectionsPerServer { get; set; }
    public WebProxyConfigDto? WebProxy { get; set; }
    public bool? EnableMultipleHttp2Connections { get; set; }
    public string? RequestHeaderEncoding { get; set; }
    public string? ResponseHeaderEncoding { get; set; }
}
