namespace Qorpe.BuildingBlocks.Multitenancy;

public interface ITenantAccessor
{
    TenantContext? Current { get; }
}