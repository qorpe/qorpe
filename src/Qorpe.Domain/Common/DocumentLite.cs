using LiteDB;

namespace Qorpe.Domain.Common;

public class DocumentLite : Document
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();
}
