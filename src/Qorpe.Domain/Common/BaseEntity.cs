using MongoDB.Bson.Serialization.Attributes;

namespace Qorpe.Domain.Common;

public abstract class BaseEntity
{
    [BsonId]
    public Guid Id { get; set; }
}
