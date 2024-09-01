using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qorpe.Domain.Constants;

namespace Qorpe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Database Configuration(s)
        var databaseProvider = configuration["DatabaseProvider"] ?? DatabaseProviders.LiteDB;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //
        #endregion

        return services;
    }
}
