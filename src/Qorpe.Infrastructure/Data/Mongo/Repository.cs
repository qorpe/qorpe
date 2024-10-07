using MongoDB.Bson;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Attributes;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Mongo;

public class Repository<TDocument>(IMongoDatabase database) : IRepository<TDocument> where TDocument : Document
{
    private readonly IMongoCollection<TDocument> _collection 
        = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));

    private static string GetCollectionName(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(CollectionNameAttribute), false)
                            .Cast<CollectionNameAttribute>()
                            .FirstOrDefault();
        return attribute?.CollectionName ?? type.Name.ToLower();
    }

    public IEnumerable<TDocument> Load()
    {
        return _collection.Find(_ => true).ToList();
    }

    public async Task<IEnumerable<TDocument>> LoadAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToList();
    }

    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        var sortDefinition = isAscending
            ? Builders<TDocument>.Sort.Ascending(sortBy)
            : Builders<TDocument>.Sort.Descending(sortBy);

        return _collection.Find(filterExpression)
                          .Sort(sortDefinition)
                          .Skip((page - 1) * pageSize)
                          .Limit(pageSize)
                          .ToList();
    }

    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        var sortDefinition = isAscending
            ? Builders<TDocument>.Sort.Ascending(sortBy)
            : Builders<TDocument>.Sort.Descending(sortBy);

        return await _collection.Find(filterExpression)
                                .Sort(sortDefinition)
                                .Skip((page - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToList();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public virtual TDocument FindById(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return _collection.Find(filter).SingleOrDefault();
    }

    public virtual async Task<TDocument> FindByIdAsync(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public long Count(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.CountDocuments(filterExpression);
    }

    public async Task<long> CountAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.CountDocumentsAsync(filterExpression);
    }

    public virtual TDocument InsertOne(TDocument document)
    {
        if (!ObjectId.TryParse(document.Id, out _))
        {
            document.Id = ObjectId.GenerateNewId().ToString();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        _collection.InsertOne(document);
        return document;
    }

    public virtual async Task<TDocument> InsertOneAsync(TDocument document)
    {
        if (!ObjectId.TryParse(document.Id, out _))
        {
            document.Id = ObjectId.GenerateNewId().ToString();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        await _collection.InsertOneAsync(document);
        return document;
    }

    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (!ObjectId.TryParse(document.Id, out _))
            {
                document.Id = ObjectId.GenerateNewId().ToString();
            }
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
        }
        _collection.InsertMany(documents);
        return documents;
    }

    public virtual async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (!ObjectId.TryParse(document.Id, out _))
            {
                document.Id = ObjectId.GenerateNewId().ToString();
            }
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
        }
        await _collection.InsertManyAsync(documents);
        return documents;
    }

    public void ReplaceOne(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        document.UpdatedAt = DateTime.Now;
        _collection.FindOneAndReplace(filter, document);
    }

    public async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        document.UpdatedAt = DateTime.Now;
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.FindOneAndDelete(filterExpression);
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.FindOneAndDeleteAsync(filterExpression);
    }

    public void DeleteById(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        _collection.FindOneAndDelete(filter);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        await _collection.FindOneAndDeleteAsync(filter);
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.DeleteMany(filterExpression);
    }

    public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.DeleteManyAsync(filterExpression);
    }
}
