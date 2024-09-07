using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Constants;
using Mongo = Qorpe.Infrastructure.Data.Mongo;
using Lite = Qorpe.Infrastructure.Data.Lite;
using LiteDB;
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
            services.AddSingleton<IMongoClient>(provider =>
            {
                var client = new MongoClient("mongodb+srv://celikomr:TajULARyFqvRmNDT@cluster.0xru5.mongodb.net/?retryWrites=true&w=majority&appName=Cluster");
                return client;
            });

            services.AddScoped<IClusterRepository<ClusterConfig>, Mongo.ClusterRepository>();
            services.AddScoped<IRouteRepository<RouteConfig>, Mongo.RouteRepository>();
        }
        else if (databaseProvider == DatabaseProviders.LiteDB)
        {
            services.AddSingleton<ILiteDatabase>(provider => new LiteDatabase("shared.db"));
            services.AddScoped<IClusterRepository<ClusterConfig>, Lite.ClusterRepository>();
            services.AddScoped<IRouteRepository<RouteConfig>, Lite.RouteRepository>();
        }
        #endregion

        return services;
    }
}
