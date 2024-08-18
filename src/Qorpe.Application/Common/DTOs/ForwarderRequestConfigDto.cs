namespace Qorpe.Application.Common.DTOs;

public sealed class ForwarderRequestConfigDto
{
    public long? Id { get; set; }

    // public static ForwarderRequestConfig Empty { get; } = new();

    public TimeSpan? ActivityTimeout { get; set; }

    // Todo
    public Version? Version { get; set; }

    public HttpVersionPolicy? VersionPolicy { get; set; }

    public bool? AllowResponseBuffering { get; set; }

    // Foreign Key
    public long? ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
