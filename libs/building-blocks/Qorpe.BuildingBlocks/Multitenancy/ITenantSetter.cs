namespace Qorpe.BuildingBlocks.Multitenancy;

public interface ITenantSetter
{
    bool TrySet(TenantContext ctx); 
    bool TryEnrich(long? id = null, string? key = null);
    bool IsSet { get; }
}