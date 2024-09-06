using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Constants;
using Mongo = Qorpe.Infrastructure.Data.Mongo;
using Lite = Qorpe.Infrastructure.Data.Lite;
using LiteDB;
using Mongo_Entities = Qorpe.Domain.Entities.Mongo;
using Lite_Entities = Qorpe.Domain.Entities.Lite;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Database Configuration(s)
        var databaseProvider = configuration["DatabaseProvider"] ?? DatabaseProviders.LiteDB;
        // var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (databaseProvider == DatabaseProviders.MongoDB)
        {
            services.AddSingleton(provider =>
            {
                var client = new MongoClient("mongodb://localhost:27017");
                return client.GetDatabase("your-database");
            });

            services.AddScoped<IClusterRepository<Mongo_Entities.ClusterConfig>, Mongo.ClusterRepository>();
            services.AddScoped<IRepository<Mongo_Entities.RouteConfig>, Mongo.RouteRepository>();
        }
        else if (databaseProvider == DatabaseProviders.LiteDB)
        {
            services.AddSingleton<ILiteDatabase>(provider => new LiteDatabase("mydatabase.db"));
            services.AddScoped<IClusterRepository<ClusterConfig>, Lite.ClusterRepository>();
            services.AddScoped<IRouteRepository<RouteConfig>, Lite.RouteRepository>();
        }
        #endregion

        return services;
    }
}
