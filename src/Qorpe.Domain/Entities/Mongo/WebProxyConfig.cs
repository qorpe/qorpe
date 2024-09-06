namespace Qorpe.Domain.Entities.Mongo;

public sealed class WebProxyConfig
{
    public Uri? Address { get; set; }
    public bool? BypassOnLocal { get; set; }
    public bool? UseDefaultCredentials { get; set; }
}
