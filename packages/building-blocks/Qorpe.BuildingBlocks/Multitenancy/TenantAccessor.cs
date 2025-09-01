namespace Qorpe.BuildingBlocks.Multitenancy;

public sealed class TenantAccessor : ITenantAccessor, ITenantSetter
{
    private TenantContext? _cur;
    public TenantContext? Current => _cur;
    public bool IsSet => _cur is not null;

    public bool TrySet(TenantContext ctx)
        => Interlocked.CompareExchange(ref _cur, ctx, null) == null;

    public bool TryEnrich(long? id = null, string? key = null)
    {
        if (_cur is null && !string.IsNullOrWhiteSpace(key))
        {
            _cur = new TenantContext(id, key);
            return true;
        }

        var cur = _cur;
        if (cur is null) return false;

        if (!string.IsNullOrWhiteSpace(key) &&
            !string.Equals(cur.Key, key, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!id.HasValue || cur.Id is not null) return false;
        _cur = cur with { Id = id.Value };
        return true;
    }
}

