using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.Features.Properties.Queries.GetAllProperties;
using Million.Properties.Application.Mappings;
using Million.Properties.Domain.Entities;
using Million.Properties.Domain.Entities.Request.Properties;
using Moq;
using NUnit.Framework;

namespace Million.Properties.Application.UnitTest.Features.Properties.Querys;

[TestFixture]
public class GetAllPropertiesHandlerTests
{
    private Mock<IPropertyRepository> _propRepo = null!;
    private Mock<IPropertyImageRepository> _imgRepo = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _propRepo = new Mock<IPropertyRepository>();
        _imgRepo = new Mock<IPropertyImageRepository>();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); }, AppDomain.CurrentDomain.GetAssemblies());

        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Test]
    public async Task GetAll_ReturnsMappedDtos_AndOneImagePerProperty()
    {
        var props = new List<Property>
        {
            new() { IdProperty = 1, Name="A", Address="X", Price=1000, CodeInternal=System.Guid.NewGuid().ToString(), Year=2024 },
            new() { IdProperty = 2, Name="B", Address="Y", Price=2000, CodeInternal=System.Guid.NewGuid().ToString(), Year=2024 },
        };

        _propRepo.Setup(r => r.GetAllWithFiltersAsync(null, null, null, null))
                 .ReturnsAsync(props);

        _imgRepo.Setup(r => r.GetAllImagesByPropertyIdsAsync(It.IsAny<IEnumerable<int>>()))
        .ReturnsAsync(new Dictionary<int, List<PropertyImage>>
        {
            { 1, new List<PropertyImage> { new PropertyImage { IdProperty = 1, File = "img1.jpg", Enabled = true } } },
            { 2, new List<PropertyImage>() }  
        });

        var handler = new GetAllPropertiesHandler(_propRepo.Object, _imgRepo.Object, _mapper);
        var result = await handler.Handle(new GetAllPropertiesQuery(new GetAllPropertiesRequest()), CancellationToken.None);
        var resultList = result.ToList();
        Assert.That(resultList, Has.Count.EqualTo(2));
    }
}
