
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.Features.Properties.Commands.CreateProperty;
using Million.Properties.Application.Mappings;
using Million.Properties.Domain.Entities;
using Million.Properties.Domain.Entities.Request;
using Moq;
using NUnit.Framework;

namespace Million.Properties.Application.UnitTest.Features.Properties.Commands;

[TestFixture]
public class CreatePropertyHandlerTests
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

        _propRepo.Setup(r => r.AddAsync(It.IsAny<Property>()))
                 .Returns(Task.CompletedTask);

        _imgRepo.Setup(r => r.AddAsync(It.IsAny<PropertyImage>()))
                .Returns(Task.CompletedTask);
    }



    [Test]
    public async Task CreateProperty_WhenFileIsEmpty_DoesNotCreateImage()
    {
        var request = new CreatePropertyRequest
        {
            Name = "Casa A",
            Address = "Calle 1",
            Price = 1000M,
            File = "" // vacío
        };

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Casa A"));
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Never);
    }

    [Test]
    public async Task CreateProperty_WhenFilePresent_CreatesImage()
    {
        var request = new CreatePropertyRequest
        {
            Name = "Casa B",
            Address = "Calle 2",
            Price = 2000M,
            File = "data:image/png;base64,AAA..."
        };

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Once);
    }
}
