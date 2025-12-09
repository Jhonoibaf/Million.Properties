using Microsoft.EntityFrameworkCore;
using Million.Properties.Infrastructure.Persistence;
using NUnit.Framework;
using Testcontainers.MsSql;

namespace Million.Properties.IntegrationTest;

[TestFixture]
public class SqlServerConnectivityTests
{
    private MsSqlContainer _sqlContainer = null!;
    private PropertiesDbContext _dbContext = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong@Passw0rd")
            .Build();

        await _sqlContainer.StartAsync();
    }

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<PropertiesDbContext>()
            .UseSqlServer(_sqlContainer.GetConnectionString())
            .Options;

        _dbContext = new PropertiesDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _sqlContainer.DisposeAsync();
    }

    [Test]
    public async Task CanConnectToSqlServer()
    {
        var canConnect = await _dbContext.Database.CanConnectAsync();
        Assert.That(canConnect, Is.True);
    }

    [Test]
    public async Task DatabaseIsCreatedSuccessfully()
    {
        var tables = await _dbContext.Database
            .SqlQueryRaw<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'")
            .ToListAsync();

        Assert.That(tables, Is.Not.Empty);
        Assert.That(tables, Does.Contain("Properties"));
        Assert.That(tables, Does.Contain("Owners"));
        Assert.That(tables, Does.Contain("PropertyImages"));
    }
}
