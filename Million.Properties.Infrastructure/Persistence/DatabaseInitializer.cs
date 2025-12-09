using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Million.Properties.Infrastructure.Persistence;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken ct = default);
}

public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly PropertiesDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(PropertiesDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        try
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync(ct);
                _logger.LogInformation("Migraciones aplicadas exitosamente");
            }
            else
            {
                _logger.LogInformation("No hay migraciones pendientes");
            }

            var canConnect = await _context.Database.CanConnectAsync(ct);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
