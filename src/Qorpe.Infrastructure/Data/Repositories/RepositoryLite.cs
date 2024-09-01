using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Repositories;

public class RepositoryLite<TDocument> : IRepository<TDocument>
    where TDocument : Document
{
    public IQueryable<TDocument> AsQueryable()
    {
        throw new NotImplementedException();
    }

    public void DeleteById(string id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        throw new NotImplementedException();
    }

    public TDocument FindById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<TDocument> FindByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }

    public void InsertMany(ICollection<TDocument> documents)
    {
        throw new NotImplementedException();
    }

    public Task InsertManyAsync(ICollection<TDocument> documents)
    {
        throw new NotImplementedException();
    }

    public void InsertOne(TDocument document)
    {
        throw new NotImplementedException();
    }

    public Task InsertOneAsync(TDocument document)
    {
        throw new NotImplementedException();
    }

    public void ReplaceOne(TDocument document)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceOneAsync(TDocument document)
    {
        throw new NotImplementedException();
    }
}
