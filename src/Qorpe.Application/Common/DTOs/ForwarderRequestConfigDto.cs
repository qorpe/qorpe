namespace Qorpe.Application.Common.DTOs;

public sealed class ForwarderRequestConfigDto
{
    public TimeSpan? ActivityTimeout { get; set; }
    public Version? Version { get; set; }
    public HttpVersionPolicy? VersionPolicy { get; set; }
    public bool? AllowResponseBuffering { get; set; }
}
