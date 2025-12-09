using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence;

public class PropertiesDbContext (DbContextOptions<PropertiesDbContext> options): DbContext (options)
{
    #region Entities
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyTrace> PropertyTraces { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
