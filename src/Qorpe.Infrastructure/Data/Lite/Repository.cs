using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Attributes;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Lite;

public class Repository<TDocument>(ILiteDatabase database) : IRepository<TDocument> where TDocument : Document
{
    private readonly ILiteCollection<TDocument> _collection 
        = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));

    private static string GetCollectionName(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(CollectionNameAttribute), false)
                            .Cast<CollectionNameAttribute>()
                            .FirstOrDefault();
        return attribute?.CollectionName ?? type.Name.ToLower();
    }

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.FindAll().AsQueryable();
    }

    public virtual IEnumerable<TDocument> Load()
    {
        return _collection.FindAll().ToList();
    }

    public async virtual Task<IEnumerable<TDocument>> LoadAsync()
    {
        return await Task.FromResult(Load());
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).AsEnumerable();
    }

    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(FilterBy(filterExpression));
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        var comparer = new PropertyComparer<TDocument>(sortBy, isAscending);

        var result = _collection.Find(filterExpression)
                                .OrderBy(doc => doc, comparer)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .AsEnumerable();

        return result;
    }

    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        return await Task.FromResult(FilterBy(filterExpression, page, pageSize, sortBy, isAscending));
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        var result = _collection.Find(filterExpression).AsEnumerable();
        return result.Select(projectionExpression.Compile()).AsEnumerable();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.FindOne(filterExpression);
    }

    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(FindOne(filterExpression));
    }

    public virtual TDocument FindById(string id)
    {
        return _collection.FindById(id);
    }

    public virtual async Task<TDocument> FindByIdAsync(string id)
    {
        return await Task.FromResult(FindById(id));
    }

    public virtual long Count(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Count(filterExpression);
    }

    public virtual async Task<long> CountAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(Count(filterExpression));
    }

    public virtual TDocument InsertOne(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            document.Id = Guid.NewGuid().ToString();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        _collection.Insert(document);
        return document;
    }

    public virtual async Task<TDocument> InsertOneAsync(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            document.Id = Guid.NewGuid().ToString();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        return await Task.FromResult(InsertOne(document));
    }

    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
            }
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
        }
        _collection.Insert(documents);
        return documents;
    }

    public virtual async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
            }
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
        }
        return await Task.FromResult(InsertMany(documents));
    }

    public void ReplaceOne(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            throw new ArgumentException("Document ID cannot be null or empty.");
        }
        document.UpdatedAt = DateTime.Now;
        _collection.Update(document);
    }

    public virtual Task ReplaceOneAsync(TDocument document)
    {
        return Task.Run(() => ReplaceOne(document));
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var document = _collection.FindOne(filterExpression);
        if (document != null)
        {
            _collection.Delete(document.Id);
        }
    }

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => DeleteOne(filterExpression));
    }

    public void DeleteById(string id)
    {
        _collection.Delete(id);
    }

    public Task DeleteByIdAsync(string id)
    {
        return Task.Run(() => DeleteById(id));
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.DeleteMany(filterExpression);
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => DeleteMany(filterExpression));
    }
}
