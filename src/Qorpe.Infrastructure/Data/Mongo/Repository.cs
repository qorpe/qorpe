using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Bson;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Attributes;
using Qorpe.Domain.Common;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Mongo;

/// <summary>
/// Generic repository class for managing MongoDB operations for documents.
/// This repository works with a specific type of document.
/// </summary>
/// <typeparam name="TDocument">Type of the document, which must inherit from the Document class.</typeparam>
public class Repository<TDocument> : IRepository<TDocument>
    where TDocument : Document
{
    /// <summary>
    /// MongoDB database instance.
    /// </summary>
    private readonly IMongoDatabase _database;

    /// <summary>
    /// MongoDB collection for the specified document type.
    /// </summary>
    private readonly IMongoCollection<TDocument> _collection;

    /// <summary>
    /// Collection name for the MongoDB collection.
    /// It is derived from the CollectionName attribute or the class name.
    /// </summary>
    private readonly string _collectionName = GetCollectionName(typeof(TDocument));

    /// <summary>
    /// The name of the MongoDB database. This value is extracted from the HTTP request headers.
    /// </summary>
    private readonly string? _databaseName;

    /// <summary>
    /// The tenant ID for multi-tenant applications. It is extracted from the HTTP request headers.
    /// </summary>
    private readonly string? _tenantId;

    /// <summary>
    /// Constructor for the Repository class. Initializes the MongoDB database and collection.
    /// </summary>
    /// <param name="mongoClient">MongoDB client used to interact with the database.</param>
    /// <param name="httpContextAccessor">Accessor to retrieve the HTTP context for tenant and database name.</param>
    public Repository(IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor)
    {
        // Ensure that the HTTP context and headers are not null
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);

        // Extract database name and tenant ID from request headers
        _databaseName = httpContextAccessor?.HttpContext.Request?.Headers["DatabaseName"].ToString();
        _tenantId = httpContextAccessor?.HttpContext.Request.Headers["TenantId"].ToString();

        // Ensure tenant ID is provided, as it's crucial for multi-tenancy
        ArgumentNullException.ThrowIfNull(_tenantId);

        // If no database name is provided, use 'shared' as the default database
        _database = mongoClient.GetDatabase(string.IsNullOrEmpty(_databaseName) ? "shared" : _databaseName);

        // Initialize the MongoDB collection for the given document type
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
    /// Adds a tenant-specific filter to the provided filter definition, ensuring that
    /// the query is restricted to documents belonging to the current tenant.
    /// </summary>
    /// <param name="filter">The original filter definition used to filter documents.</param>
    /// <returns>A combined filter definition that includes the tenant filter.</returns>
    private FilterDefinition<TDocument> AddTenantFilter(FilterDefinition<TDocument> filter)
    {
        var tenantFilter = Builders<TDocument>.Filter.Eq(doc => doc.TenantId, _tenantId);
        return Builders<TDocument>.Filter.And(filter, tenantFilter);
    }

    /// <summary>
    /// Returns an IQueryable of documents from the MongoDB collection.
    /// </summary>
    /// <returns>IQueryable of TDocument.</returns>
    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    /// <summary>
    /// Filters documents based on a specified filter expression.
    /// </summary>
    /// <param name="filterExpression">Expression to filter the documents.</param>
    /// <returns>A list of documents that match the filter.</returns>
    public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        return _collection.Find(filterWithTenant).ToEnumerable();
    }

    /// <summary>
    /// Filters documents based on a filter expression and projects the result into another type.
    /// </summary>
    /// <typeparam name="TProjected">The type to project the result into.</typeparam>
    /// <param name="filterExpression">Expression to filter the documents.</param>
    /// <param name="projectionExpression">Expression to project the documents into another type.</param>
    /// <returns>A list of projected results.</returns>
    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        return _collection.Find(filterWithTenant).Project(projectionExpression).ToEnumerable();
    }

    /// <summary>
    /// Finds a single document based on a specified filter expression.
    /// </summary>
    /// <param name="filterExpression">Expression to filter the documents.</param>
    /// <returns>The first document that matches the filter.</returns>
    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        return _collection.Find(filterWithTenant).FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously finds a single document based on a specified filter expression.
    /// </summary>
    /// <param name="filterExpression">Expression to filter the documents.</param>
    /// <returns>A task that represents the asynchronous operation, with the result being the found document.</returns>
    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        return await _collection.Find(filterWithTenant).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Finds a document by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the document to find.</param>
    /// <returns>The document that matches the specified ID, or null if not found.</returns>
    public virtual TDocument FindById(string id)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, id));
        return _collection.Find(filter).SingleOrDefault();
    }

    /// <summary>
    /// Asynchronously finds a document by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the document to find.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the found document.</returns>
    public virtual async Task<TDocument> FindByIdAsync(string id)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, id));
        return await _collection.Find(filter).SingleOrDefaultAsync();
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
    /// <remarks>
    /// This method allows for filtering the documents in the collection based on a C# expression. 
    /// It is designed to work with any document type <typeparamref name="TDocument"/> in the MongoDB collection. 
    /// The filter expression is expected to define a logical condition for which documents to include in the count operation.
    /// </remarks>
    public long Count(Expression<Func<TDocument, bool>> filterExpression)
    {
        // Count the documents in the collection that match the filter
        return _collection.CountDocuments(filterExpression);
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
    /// <remarks>
    /// This method allows for filtering the documents in the collection based on a C# expression. 
    /// It is designed to work with any document type <typeparamref name="TDocument"/> in the MongoDB collection. 
    /// The filter expression is expected to define a logical condition for which documents to include in the count operation.
    /// </remarks>
    public async Task<long> CountAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        // Count the documents in the collection that match the filter
        return await _collection.CountDocumentsAsync(filterExpression);
    }

    /// <summary>
    /// Inserts a new document into the collection.
    /// </summary>
    /// <param name="document">The document to insert.</param>
    /// <returns>The inserted document.</returns>
    public virtual TDocument InsertOne(TDocument document)
    {
        if (!ObjectId.TryParse(document.Id, out _))
        {
            document.Id = ObjectId.GenerateNewId().ToString();
        }
        document.TenantId = _tenantId;

        _collection.InsertOne(document);
        return document;
    }

    /// <summary>
    /// Asynchronously inserts a new document into the collection.
    /// </summary>
    /// <param name="document">The document to insert.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the inserted document.</returns>
    public virtual async Task<TDocument> InsertOneAsync(TDocument document)
    {
        if (!ObjectId.TryParse(document.Id, out _))
        {
            document.Id = ObjectId.GenerateNewId().ToString();
        }
        document.TenantId = _tenantId;

        await _collection.InsertOneAsync(document);
        return document;
    }

    /// <summary>
    /// Inserts multiple documents into the collection.
    /// </summary>
    /// <param name="documents">The documents to insert.</param>
    /// <returns>The inserted documents.</returns>
    public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (!ObjectId.TryParse(document.Id, out _))
            {
                document.Id = ObjectId.GenerateNewId().ToString();
            }
            document.TenantId = _tenantId;
        }

        _collection.InsertMany(documents);
        return documents;
    }

    /// <summary>
    /// Asynchronously inserts multiple documents into the collection.
    /// </summary>
    /// <param name="documents">The documents to insert.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the inserted documents.</returns>
    public virtual async Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
    {
        foreach (var document in documents)
        {
            if (!ObjectId.TryParse(document.Id, out _))
            {
                document.Id = ObjectId.GenerateNewId().ToString();
            }
            document.TenantId = _tenantId;
        }

        await _collection.InsertManyAsync(documents);
        return documents;
    }

    /// <summary>
    /// Replaces an existing document with a new one.
    /// </summary>
    /// <param name="document">The document to replace.</param>
    public void ReplaceOne(TDocument document)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id));
        _collection.FindOneAndReplace(filter, document);
    }

    /// <summary>
    /// Asynchronously replaces an existing document with a new one.
    /// </summary>
    /// <param name="document">The document to replace.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ReplaceOneAsync(TDocument document)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id));
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    /// <summary>
    /// Deletes a document from the collection based on the filter expression.
    /// </summary>
    /// <param name="filterExpression">The expression used to filter the documents to be deleted.</param>
    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        _collection.FindOneAndDelete(filterWithTenant);
    }

    /// <summary>
    /// Asynchronously deletes a document from the collection based on the filter expression.
    /// </summary>
    /// <param name="filterExpression">The expression used to filter the documents to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        await _collection.FindOneAndDeleteAsync(filterWithTenant);
    }

    /// <summary>
    /// Deletes a document from the collection based on its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    public void DeleteById(string id)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, id));
        _collection.FindOneAndDelete(filter);
    }

    /// <summary>
    /// Asynchronously deletes a document from the collection based on its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteByIdAsync(string id)
    {
        var filter = AddTenantFilter(Builders<TDocument>.Filter.Eq(doc => doc.Id, id));
        await _collection.FindOneAndDeleteAsync(filter);
    }

    /// <summary>
    /// Deletes multiple documents from the collection based on the provided filter expression.
    /// </summary>
    /// <param name="filterExpression">The expression used to filter the documents to be deleted.</param>
    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        _collection.DeleteMany(filterWithTenant);
    }

    /// <summary>
    /// Asynchronously deletes multiple documents from the collection based on the provided filter expression.
    /// </summary>
    /// <param name="filterExpression">The expression used to filter the documents to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        var filterWithTenant = AddTenantFilter(Builders<TDocument>.Filter.Where(filterExpression));
        await _collection.DeleteManyAsync(filterWithTenant);
    }
}
