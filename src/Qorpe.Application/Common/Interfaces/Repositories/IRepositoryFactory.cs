using Qorpe.Domain.Common;

namespace Qorpe.Application.Common.Interfaces.Repositories;

public interface IRepositoryFactory
{
    IRepository<TDocument> CreateRepository<TDocument>(string tenantId)
        where TDocument : Document;
}
