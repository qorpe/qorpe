using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Qorpe.Domain.Common;

public class DocumentMongo : Document
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public new string? Id { get; set; }
}
