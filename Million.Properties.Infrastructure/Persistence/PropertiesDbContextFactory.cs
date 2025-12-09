using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Million.Properties.Infrastructure.Persistence;

public class PropertiesDbContextFactory : IDesignTimeDbContextFactory<PropertiesDbContext>
{
    public PropertiesDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Million.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<PropertiesDbContextFactory>(optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<PropertiesDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(PropertiesDbContext).Assembly.GetName().Name);
        });

        return new PropertiesDbContext(optionsBuilder.Options);
    }
}