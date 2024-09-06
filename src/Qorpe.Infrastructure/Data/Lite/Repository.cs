using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Lite;

public class Repository<TDocument>(ILiteDatabase database) : IRepository<TDocument>
    where TDocument : Document
{
    private readonly ILiteCollection<TDocument> _collection 
        = database.GetCollection<TDocument>($"tenant_{typeof(TDocument).Name}"); // Todo - Tenant Id

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.FindAll().AsQueryable();
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(ConvertExpression(filterExpression)).AsEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        var result = _collection.Find(ConvertExpression(filterExpression)).AsEnumerable();
        return result.Select(projectionExpression.Compile()).AsEnumerable();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.FindOne(ConvertExpression(filterExpression));
    }

    public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.FromResult(_collection.FindOne(ConvertExpression(filterExpression)));
    }

    public virtual TDocument FindById(string id)
    {
        return _collection.FindById(id);
    }

    public virtual Task<TDocument> FindByIdAsync(string id)
    {
        return Task.FromResult(_collection.FindById(id));
    }

    public virtual TDocument InsertOne(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            document.Id = Guid.NewGuid().ToString();
        }
        _collection.Insert(document);
        return document;
    }

    public virtual Task<TDocument> InsertOneAsync(TDocument document)
    {
        return Task.Run(() =>
        {
            _collection.Insert(document);
            return document;
        });
    }

    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        _collection.Insert(documents);
        return documents;
    }

    public virtual Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        return Task.Run(() =>
        {
            _collection.Insert(documents);
            return documents;
        });
    }

    public void ReplaceOne(TDocument document)
    {
        _collection.Update(document);
    }

    public virtual Task ReplaceOneAsync(TDocument document)
    {
        return Task.Run(() => _collection.Update(document));
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filter = ConvertExpression(filterExpression);
        var document = _collection.FindOne(filter);
        if (document != null)
        {
            _collection.Delete(document.Id);
        }
    }

    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() =>
        {
            var filter = ConvertExpression(filterExpression);
            var document = _collection.FindOne(filter);
            if (document != null)
            {
                _collection.Delete(document.Id);
            }
        });
    }

    public void DeleteById(string id)
    {
        _collection.Delete(id);
    }

    public Task DeleteByIdAsync(string id)
    {
        return Task.Run(() => _collection.Delete(id));
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filter = ConvertExpression(filterExpression);
        _collection.DeleteMany(filter);
    }

    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() =>
        {
            var filter = ConvertExpression(filterExpression);
            _collection.DeleteMany(filter);
        });
    }

    private static BsonExpression ConvertExpression<T>(Expression<Func<T, bool>> expression)
    {
        if (expression.Body is BinaryExpression binaryExpression)
        {
            if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                var left = (MemberExpression)binaryExpression.Left;
                var right = (ConstantExpression)binaryExpression.Right;
                return Query.EQ(left.Member.Name, right?.Value?.ToString());
            }
        }

        throw new NotSupportedException("This type of expression is not supported.");
    }
}
