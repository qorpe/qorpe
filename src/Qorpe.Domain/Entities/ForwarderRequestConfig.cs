namespace Qorpe.Domain.Entities;

public sealed class ForwarderRequestConfig
{
    public long Id { get; set; }
    
    // public static ForwarderRequestConfig Empty { get; } = new();

    public TimeSpan? ActivityTimeout { get; init; }

    public Version? Version { get; init; }

    public HttpVersionPolicy? VersionPolicy { get; init; }

    public bool? AllowResponseBuffering { get; init; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
