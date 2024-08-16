namespace Qorpe.Domain.Entities;

public sealed class SessionAffinityConfig
{
    public bool? Enabled { get; init; }

    public string? Policy { get; init; }

    public string? FailurePolicy { get; init; }

    public string AffinityKeyName { get; init; } = default!;

    public SessionAffinityCookieConfig? Cookie { get; init; }
}
