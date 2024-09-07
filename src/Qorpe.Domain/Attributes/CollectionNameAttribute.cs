namespace Qorpe.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CollectionNameAttribute(string collectionName) : Attribute
{
    public string CollectionName { get; } = collectionName;
}
