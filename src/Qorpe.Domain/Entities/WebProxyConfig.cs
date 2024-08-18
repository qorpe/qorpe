namespace Qorpe.Domain.Entities;

public sealed class WebProxyConfig
{
    public long? Id { get; set; }

    public Uri? Address { get; set; }

    public bool? BypassOnLocal { get; set; }

    public bool? UseDefaultCredentials { get; set; }

    // Foreign Key
    public long? HttpClientConfigId { get; set; }
}
