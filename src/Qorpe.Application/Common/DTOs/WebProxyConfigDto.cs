namespace Qorpe.Application.Common.DTOs;

public sealed class WebProxyConfigDto
{
    public Uri? Address { get; set; }
    public bool? BypassOnLocal { get; set; }
    public bool? UseDefaultCredentials { get; set; }
}
