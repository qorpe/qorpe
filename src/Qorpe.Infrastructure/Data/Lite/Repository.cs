using LiteDB;
using Microsoft.AspNetCore.Http;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Attributes;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Lite;

/// <summary>
/// Repository for handling CRUD operations with LiteDB.
/// </summary>
/// <typeparam name="TDocument">The type of the document.</typeparam>
public class Repository<TDocument> : IRepository<TDocument>
    where TDocument : Document
{
    /// <summary>
    /// The LiteDB database instance used for data access.
    /// </summary>
    private readonly ILiteDatabase _database;

    /// <summary>
    /// The LiteDB collection used for CRUD operations on documents of type <typeparamref name="TDocument"/>.
    /// </summary>
    private readonly ILiteCollection<TDocument> _collection;

    /// <summary>
    /// The name of the collection, derived from the <see cref="CollectionNameAttribute"/> or defaulting to the document type name.
    /// </summary>
    private readonly string _collectionName = GetCollectionName(typeof(TDocument));

    /// <summary>
    /// The name of the database to be used. If null or empty, the default database is used.
    /// </summary>
    private readonly string? _databaseName;

    /// <summary>
    /// The tenant ID used for tenant-specific filtering and management.
    /// </summary>
    private readonly string? _tenantId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TDocument}"/> class.
    /// </summary>
    /// <param name="database">The LiteDB database instance.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor for obtaining tenant and database names.</param>
    public Repository(ILiteDatabase database, IHttpContextAccessor httpContextAccessor)
    {
        // Ensure that the HTTP context and headers are not null
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext, nameof(httpContextAccessor.HttpContext));

        // Extract database name and tenant ID from request headers
        _databaseName = httpContextAccessor.HttpContext.Request.Headers["DatabaseName"].ToString();
        _tenantId = httpContextAccessor.HttpContext.Request.Headers["TenantId"].ToString();

        // Ensure tenant ID is provided, as it's crucial for multi-tenancy
        ArgumentNullException.ThrowIfNull(_tenantId, nameof(_tenantId));

        // If no database name is provided, use the default 'shared' database
        _database = string.IsNullOrEmpty(_databaseName) ? database : new LiteDatabase($"{_databaseName}.db");

        // Collection name is customized
        _collection = _database.GetCollection<TDocument>(_collectionName);
    }

    /// <summary>
    /// Gets the collection name from the CollectionName attribute, or defaults to the class name if not provided.
    /// </summary>
    /// <param name="type">Type of the document.</param>
    /// <returns>Collection name as a string.</returns>
    private static string GetCollectionName(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(CollectionNameAttribute), false)
                            .Cast<CollectionNameAttribute>()
                            .FirstOrDefault();
        return attribute?.CollectionName ?? type.Name.ToLower();
    }

    /// <summary>
    /// Returns a queryable collection of documents.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TDocument}"/> of documents.</returns>
    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.FindAll().AsQueryable();
    }

    /// <summary>
    /// Filters documents based on a given filter expression and includes tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to apply.</param>
    /// <returns>An <see cref="IEnumerable{TDocument}"/> of documents that match the filter.</returns>
    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        // Apply tenant filtering along with the provided filter expression
        var filterWithTenant = CombineFilterWithTenant(filterExpression);

        // Perform the query asynchronously: filter the documents and retrieve them as an enumerable collection
        // Convert the filter expression to a format compatible with LiteDB
        var result = _collection.Find(ConvertExpression(filterWithTenant)).AsEnumerable();

        // Return the filtered documents
        return result;
    }

    /// <summary>
    /// Asynchronously filters the collection based on a given expression, applying tenant filtering before performing the query.
    /// This method retrieves all documents that match the filter criteria without pagination or sorting.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document in the collection.</typeparam>
    /// <param name="filterExpression">An expression used to filter the documents in the collection.</param>
    /// <returns>A task representing an asynchronous operation that returns an enumerable collection of filtered documents.</returns>
    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(FilterBy(filterExpression));
    }

    /// <summary>
    /// Filters the collection based on a given expression, paginates the results, and sorts them by the specified field.
    /// Adds tenant filtering automatically before performing the query.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document in the collection.</typeparam>
    /// <param name="filterExpression">An expression used to filter the documents in the collection.</param>
    /// <param name="page">The page number for pagination (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field by which the documents will be sorted.</param>
    /// <param name="isAscending">Determines the sort order: true for ascending, false for descending.</param>
    /// <returns>A list of filtered, sorted, and paginated documents.</returns>
    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        // Apply tenant filtering along with the provided filter expression
        var filterWithTenant = CombineFilterWithTenant(filterExpression);

        // Use a custom comparer to sort documents
        var comparer = new PropertyComparer<TDocument>(sortBy, isAscending);

        // Perform pagination: skip the records from previous pages and limit the number of results to pageSize
        var result = _collection.Find(ConvertExpression(filterWithTenant))
                                .OrderBy(doc => doc, comparer)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .AsEnumerable();

        return result;
    }

    /// <summary>
    /// Asynchronously filters the collection based on a given expression, paginates the results, and sorts them by the specified field.
    /// Adds tenant filtering automatically before performing the query.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document in the collection.</typeparam>
    /// <param name="filterExpression">An expression used to filter the documents in the collection.</param>
    /// <param name="page">The page number for pagination (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field by which the documents will be sorted.</param>
    /// <param name="isAscending">Determines the sort order: true for ascending, false for descending.</param>
    /// <returns>A task representing an asynchronous operation that returns a list of filtered, sorted, and paginated documents.</returns>
    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression, int page, int pageSize, string sortBy, bool isAscending)
    {
        return await Task.FromResult(FilterBy(filterExpression, page, pageSize, sortBy, isAscending));
    }

    /// <summary>
    /// Filters documents based on a given filter expression and projection expression, including tenant ID in the filter.
    /// </summary>
    /// <typeparam name="TProjected">The type of the projected result.</typeparam>
    /// <param name="filterExpression">The filter expression to apply.</param>
    /// <param name="projectionExpression">The projection expression to apply.</param>
    /// <returns>An <see cref="IEnumerable{TProjected}"/> of projected results.</returns>
    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        var filterWithTenant = CombineFilterWithTenant(filterExpression);
        var result = _collection.Find(ConvertExpression(filterWithTenant)).AsEnumerable();
        return result.Select(projectionExpression.Compile()).AsEnumerable();
    }

    /// <summary>
    /// Finds a single document that matches the given filter expression and includes tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match.</param>
    /// <returns>The matching document, or null if no document is found.</returns>
    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = CombineFilterWithTenant(filterExpression);
        return _collection.FindOne(ConvertExpression(filterWithTenant));
    }

    /// <summary>
    /// Asynchronously finds a single document that matches the given filter expression and includes tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the matching document.</returns>
    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(FindOne(filterExpression));
    }

    /// <summary>
    /// Finds a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to find.</param>
    /// <returns>The document with the specified ID, or null if not found.</returns>
    public virtual TDocument FindById(string id)
    {
        return _collection.FindById(id);
    }

    /// <summary>
    /// Asynchronously finds a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to find.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the document with the specified ID.</returns>
    public virtual async Task<TDocument> FindByIdAsync(string id)
    {
        return await Task.FromResult(FindById(id));
    }

    /// <summary>
    /// Synchronously counts the number of documents in the collection that match the specified filter expression.
    /// </summary>
    /// <param name="filterExpression">
    /// A lambda expression of type <see cref="Expression{Func{TDocument, bool}}"/> used to filter the documents 
    /// in the collection. The expression defines the criteria for which documents to count.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation. 
    /// The task result contains the count of documents in the collection that satisfy the filter criteria.
    /// </returns>
    public virtual long Count(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = CombineFilterWithTenant(filterExpression);
        // Count the documents in the collection that match the filter
        return _collection.Count(ConvertExpression(filterWithTenant));
    }

    /// <summary>
    /// Asynchronously counts the number of documents in the collection that match the specified filter expression.
    /// </summary>
    /// <param name="filterExpression">
    /// A lambda expression of type <see cref="Expression{Func{TDocument, bool}}"/> used to filter the documents 
    /// in the collection. The expression defines the criteria for which documents to count.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation. 
    /// The task result contains the count of documents in the collection that satisfy the filter criteria.
    /// </returns>
    public virtual async Task<long> CountAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Task.FromResult(Count(filterExpression));
    }

    /// <summary>
    /// Inserts a new document into the collection. Automatically generates an ID if not provided.
    /// </summary>
    /// <param name="document">The document to insert.</param>
    /// <returns>The inserted document with its ID assigned.</returns>
    public virtual TDocument InsertOne(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            document.Id = Guid.NewGuid().ToString();
        }
        document.TenantId = _tenantId;
        _collection.Insert(document);
        return document;
    }

    /// <summary>
    /// Asynchronously inserts a new document into the collection. Automatically generates an ID if not provided.
    /// </summary>
    /// <param name="document">The document to insert.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the inserted document with its ID assigned.</returns>
    public virtual async Task<TDocument> InsertOneAsync(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            document.Id = Guid.NewGuid().ToString();
        }
        document.TenantId = _tenantId;
        return await Task.FromResult(InsertOne(document));
    }

    /// <summary>
    /// Inserts multiple documents into the collection. Automatically generates IDs for documents that do not have one.
    /// </summary>
    /// <param name="documents">The documents to insert.</param>
    /// <returns>The collection of inserted documents with their IDs assigned.</returns>
    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
            }
            document.TenantId = _tenantId;
        }
        _collection.Insert(documents);
        return documents;
    }

    /// <summary>
    /// Asynchronously inserts multiple documents into the collection. Automatically generates IDs for documents that do not have one.
    /// </summary>
    /// <param name="documents">The documents to insert.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the collection of inserted documents with their IDs assigned.</returns>
    public virtual async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
            }
            document.TenantId = _tenantId;
        }
        return await Task.FromResult(InsertMany(documents));
    }

    /// <summary>
    /// Replaces an existing document in the collection with the provided document.
    /// </summary>
    /// <param name="document">The document to replace.</param>
    public void ReplaceOne(TDocument document)
    {
        if (string.IsNullOrEmpty(document.Id))
        {
            throw new ArgumentException("Document ID cannot be null or empty.");
        }
        _collection.Update(document);
    }

    /// <summary>
    /// Asynchronously replaces an existing document in the collection with the provided document.
    /// </summary>
    /// <param name="document">The document to replace.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task ReplaceOneAsync(TDocument document)
    {
        return Task.Run(() => ReplaceOne(document));
    }

    /// <summary>
    /// Deletes a single document that matches the given filter expression, including tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match the document to delete.</param>
    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = CombineFilterWithTenant(filterExpression);
        var document = _collection.FindOne(ConvertExpression(filterWithTenant));
        if (document != null)
        {
            _collection.Delete(document.Id);
        }
    }

    /// <summary>
    /// Asynchronously deletes a single document that matches the given filter expression, including tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match the document to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => DeleteOne(filterExpression));
    }

    /// <summary>
    /// Deletes a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    public void DeleteById(string id)
    {
        _collection.Delete(id);
    }

    /// <summary>
    /// Asynchronously deletes a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteByIdAsync(string id)
    {
        return Task.Run(() => DeleteById(id));
    }

    /// <summary>
    /// Deletes multiple documents that match the given filter expression, including tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match the documents to delete.</param>
    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = CombineFilterWithTenant(filterExpression);
        _collection.DeleteMany(ConvertExpression(filterWithTenant));
    }

    /// <summary>
    /// Asynchronously deletes multiple documents that match the given filter expression, including tenant ID in the filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to match the documents to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => DeleteMany(filterExpression));
    }

    /// <summary>
    /// Combines the filter expression with the tenant ID filter.
    /// </summary>
    /// <param name="filterExpression">The filter expression to combine.</param>
    /// <returns>The combined filter expression.</returns>
    private Expression<Func<TDocument, bool>> CombineFilterWithTenant(Expression<Func<TDocument, bool>> filterExpression)
    {
        // Create a parameter for the tenant filter expression
        var parameter = filterExpression.Parameters.First();

        // Create a new expression to compare the tenant ID
        var tenantIdExpression = Expression.Equal(
            Expression.Property(parameter, nameof(Document.TenantId)),
            Expression.Constant(_tenantId)
        );

        // Combine the tenant ID expression with the existing filter expression
        var combinedExpression = Expression.AndAlso(filterExpression.Body, tenantIdExpression);

        // Return the new expression
        return Expression.Lambda<Func<TDocument, bool>>(combinedExpression, parameter);
    }

    /// <summary>
    /// Converts an expression tree representing a filter condition into a LiteDB BSON query.
    /// Currently supports only equality comparisons (==) on properties.
    /// </summary>
    /// <typeparam name="T">The type of the object being queried.</typeparam>
    /// <param name="expression">The filter expression to convert.</param>
    /// <returns>A <see cref="BsonExpression"/> representing the converted query for use with LiteDB.</returns>
    /// <exception cref="NotSupportedException">Thrown when the expression type is not supported.</exception>
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
