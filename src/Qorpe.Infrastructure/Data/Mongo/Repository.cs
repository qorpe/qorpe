using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Mongo;

public class Repository<TDocument>(IMongoDatabase database) : IRepository<TDocument>
    where TDocument : Document
{
    private readonly IMongoCollection<TDocument> _collection = database.GetCollection<TDocument>(typeof(TDocument).Name);

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
    }

    public virtual TDocument FindById(string id)
    {
        // var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return _collection.Find(filter).SingleOrDefault();
    }

    public virtual Task<TDocument> FindByIdAsync(string id)
    {
        return Task.Run(() =>
        {
            // var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefaultAsync();
        });
    }


    public virtual TDocument InsertOne(TDocument document)
    {
        _collection.InsertOne(document);
        return document;
    }

    public virtual async Task<TDocument> InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
        return document;
    }

    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        _collection.InsertMany(documents);
        return documents;
    }

    public virtual async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        await _collection.InsertManyAsync(documents);
        return documents;
    }

    public void ReplaceOne(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        _collection.FindOneAndReplace(filter, document);
    }

    public virtual async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.FindOneAndDelete(filterExpression);
    }

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
    }

    public void DeleteById(string id)
    {
        // var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        _collection.FindOneAndDelete(filter);
    }

    public Task DeleteByIdAsync(string id)
    {
        return Task.Run(() =>
        {
            // var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDeleteAsync(filter);
        });
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.DeleteMany(filterExpression);
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
    }
}
