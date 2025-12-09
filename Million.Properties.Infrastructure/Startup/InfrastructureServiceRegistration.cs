using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Infrastructure.Persistence;
using Million.Properties.Infrastructure.Persistence.Repositories;

namespace Million.Properties.Infrastructure.Startup;

public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Registra todos los servicios de la capa de infraestructura
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureDatabase(services, configuration);
        ConfigureRepositories(services);
        ConfigureHealthChecks(services, configuration);

        return services;
    }

    #region Database Configuration
    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<PropertiesDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(60);
                sqlOptions.MigrationsAssembly(typeof(PropertiesDbContext).Assembly.GetName().Name);
            });

            #if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            #endif
        });

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
    }
    #endregion

    #region Repository Configuration
    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
        // services.AddScoped<IOwnerRepository, OwnerRepository>();
    }
    #endregion

    #region HealthChecks Configuration
    private static void ConfigureHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHealthChecks()
            .AddSqlServer(
                connectionString!,
                name: "sql",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "sql", "sqlserver" });
    }
    #endregion

}
