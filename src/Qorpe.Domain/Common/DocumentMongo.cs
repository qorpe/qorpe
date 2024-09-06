using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Qorpe.Domain.Common;

public class DocumentMongo : Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
