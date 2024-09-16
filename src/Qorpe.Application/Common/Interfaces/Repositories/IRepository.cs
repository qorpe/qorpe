using System.Linq.Expressions;

namespace Qorpe.Application.Common.Interfaces.Repositories;

public interface IRepository<TDocument> where TDocument : class
{
    IEnumerable<TDocument> Load();

    Task<IEnumerable<TDocument>> LoadAsync();

    IQueryable<TDocument> AsQueryable();

    IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);

    Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);

    IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending);

    Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);

    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    TDocument FindById(string id);

    Task<TDocument> FindByIdAsync(string id);

    long Count(Expression<Func<TDocument, bool>> filterExpression);

    Task<long> CountAsync(Expression<Func<TDocument, bool>> filterExpression);

    TDocument InsertOne(TDocument document);

    Task<TDocument> InsertOneAsync(TDocument document);

    ICollection<TDocument> InsertMany(ICollection<TDocument> documents);

    Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents);

    void ReplaceOne(TDocument document);

    Task ReplaceOneAsync(TDocument document);

    void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    void DeleteById(string id);

    Task DeleteByIdAsync(string id);

    void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}
