namespace Qorpe.Domain.Entities.Lite;

public sealed class ForwarderRequestConfig
{
    public TimeSpan? ActivityTimeout { get; set; }
    public Version? Version { get; set; }
    public HttpVersionPolicy? VersionPolicy { get; set; }
    public bool? AllowResponseBuffering { get; set; }
}
