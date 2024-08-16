using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qorpe.Domain.Constants;
using Qorpe.Infrastructure.Data;

namespace Qorpe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Database Configuration(s)
        var databaseProvider = configuration["DatabaseProvider"] ?? DatabaseProviders.Sqlite;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (databaseProvider.ToLower())
            {
                case DatabaseProviders.SqlServer:
                    options.UseSqlServer(connectionString);
                    break;
                case DatabaseProviders.PostgreSql:
                    options.UseNpgsql(connectionString);
                    break;
                case DatabaseProviders.MySql:
                    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
                    break;
                case DatabaseProviders.Oracle:
                    options.UseOracle(connectionString);
                    break;
                default:
                    options.UseSqlite(connectionString);
                    break;
            }
        });
        #endregion

        return services;
    }
}
