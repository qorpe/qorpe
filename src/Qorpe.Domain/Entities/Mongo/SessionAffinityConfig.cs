namespace Qorpe.Domain.Entities.Mongo;

public sealed class SessionAffinityConfig
{
    public bool? Enabled { get; set; }
    public string? Policy { get; set; }
    public string? FailurePolicy { get; set; }
    public string AffinityKeyName { get; set; } = default!;
    public SessionAffinityCookieConfig? Cookie { get; set; }
}
