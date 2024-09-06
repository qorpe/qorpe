namespace Qorpe.Application.Common.Interfaces.Repositories;

public interface IClusterRepository<TDocument> : IRepository<TDocument>
    where TDocument : class
{
}
