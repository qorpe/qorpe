namespace Qorpe.Domain.Entities;

public sealed class WebProxyConfig
{
    public Uri? Address { get; init; }

    public bool? BypassOnLocal { get; init; }

    public bool? UseDefaultCredentials { get; init; }
}
