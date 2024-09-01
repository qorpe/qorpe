using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Common;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class RepositoryFactory(ILiteDatabase database) : IRepositoryFactory
{
    public IRepository<TDocument> CreateRepository<TDocument>(string tenantId)
        where TDocument : Document
    {
        if (typeof(TDocument) == typeof(RouteConfig))
        {
            return (IRepository<TDocument>) new RouteRepository(database, tenantId);
        }
        else if (typeof(TDocument) == typeof(ClusterConfig))
        {
            return (IRepository<TDocument>)  new ClusterRepository(database, tenantId);
        }
        else
        {
            throw new NotSupportedException("Repository type not supported.");
        }
    }
}
