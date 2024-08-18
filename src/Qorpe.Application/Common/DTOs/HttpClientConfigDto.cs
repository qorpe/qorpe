﻿using System.Security.Authentication;

namespace Qorpe.Application.Common.DTOs;

public sealed class HttpClientConfigDto
{
    public long? Id { get; set; }

    // public static readonly HttpClientConfig Empty = new();

    public SslProtocols? SslProtocols { get; set; }

    public bool? DangerousAcceptAnyServerCertificate { get; set; }

    public int? MaxConnectionsPerServer { get; set; }

    public WebProxyConfigDto? WebProxy { get; set; }

    public bool? EnableMultipleHttp2Connections { get; set; }

    public string? RequestHeaderEncoding { get; set; }

    public string? ResponseHeaderEncoding { get; set; }

    // Foreign Key
    public long? ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
