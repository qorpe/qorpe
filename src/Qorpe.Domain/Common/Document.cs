using Lite = LiteDB;
using Mongo = MongoDB.Bson;

namespace Qorpe.Domain.Common;

public abstract class Document
{
    public Mongo.ObjectId? MongoId { get; set; }
    public Lite.ObjectId? LiteId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
