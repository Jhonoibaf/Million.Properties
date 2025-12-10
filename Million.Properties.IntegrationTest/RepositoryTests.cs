using Microsoft.EntityFrameworkCore;
using Million.Properties.Domain.Entities;
using Million.Properties.Infrastructure.Persistence;
using Million.Properties.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Testcontainers.MsSql;

namespace Million.Properties.IntegrationTest;

[TestFixture]
public class RepositoryTests
{
    private MsSqlContainer _sqlContainer = null!;
    private PropertiesDbContext _ctx = null!;
    private PropertyRepository _repo = null!;

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

        _ctx = new PropertiesDbContext(options);
        await _ctx.Database.EnsureCreatedAsync();
        _repo = new PropertyRepository(_ctx);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _ctx.Database.EnsureDeletedAsync();
        await _ctx.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _sqlContainer.DisposeAsync();
    }

    [Test]
    public async Task AddAndGetById_Works()
    {
        var p = new Property
        {
            Name = "Int Test House",
            Address = "Street 1",
            Price = 123m,
            CodeInternal = Guid.NewGuid().ToString(),
            Year = 2024
        };
        await _repo.AddAsync(p);

        var found = await _repo.GetByIdAsync(p.IdProperty);
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.Name, Is.EqualTo("Int Test House"));
    }

    [Test]
    public async Task Search_WithPriceRange_Works()
    {
        var list = await _repo.GetAllWithFiltersAsync(null, null, 0m, 1000000m);
        Assert.That(list, Is.Not.Null);
    }
}
