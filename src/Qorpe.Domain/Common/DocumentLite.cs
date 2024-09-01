using LiteDB;

namespace Qorpe.Domain.Common;

public class DocumentLite : Document
{
    [BsonId]
    public new string? Id { get; set; }
}
