using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.Features.Properties.Commands.CreateProperty;
using Million.Properties.Application.Mappings;
using Million.Properties.Domain.Entities;
using Million.Properties.Domain.Entities.Request;
using Million.Properties.Domain.Entities.Request.Properties;
using Moq;
using NUnit.Framework;

namespace Million.Properties.Application.UnitTest.Features.Properties.Commands;

[TestFixture]
public class CreatePropertyHandlerTests
{
    private Mock<IPropertyRepository> _propRepo = null!;
    private Mock<IPropertyImageRepository> _imgRepo = null!;
    private Mock<IUnitOfWork> _unitOfWork = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _propRepo = new Mock<IPropertyRepository>();
        _imgRepo = new Mock<IPropertyImageRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); }, AppDomain.CurrentDomain.GetAssemblies());

        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IMapper>();

        _unitOfWork.Setup(u => u.PropertyRepository).Returns(_propRepo.Object);
        _unitOfWork.Setup(u => u.PropertyImageRepository).Returns(_imgRepo.Object);
        _unitOfWork.Setup(u => u.Mapper).Returns(_mapper);

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
            InternalCode = Guid.NewGuid(),
            Year = 2024,
            IdOwner = 1,
            File = "" 
        };

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
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
            InternalCode = Guid.NewGuid(),
            Year = 2024,
            IdOwner = 1,
            File = "data:image/png;base64,AAA..."
        };

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Once);
    }

    [Test]
    public async Task CreateProperty_WhenInternalCodeIsNull_GeneratesNewGuid()
    {
        var request = new CreatePropertyRequest
        {
            Name = "Casa C",
            Address = "Calle 3",
            Price = 3000M,
            InternalCode = null, 
            Year = 2024,
            IdOwner = 1,
            File = ""
        };

        Property? capturedProperty = null;
        _propRepo.Setup(r => r.AddAsync(It.IsAny<Property>()))
                 .Callback<Property>(p => capturedProperty = p)
                 .Returns(Task.CompletedTask);

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(capturedProperty, Is.Not.Null);
        Assert.That(capturedProperty!.CodeInternal, Is.Not.Null);
        Assert.That(capturedProperty.CodeInternal, Is.Not.Empty);
        Assert.That(Guid.TryParse(capturedProperty.CodeInternal, out _), Is.True);
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
    }

    [Test]
    public async Task CreateProperty_WithAllRequiredFields_Succeeds()
    {
        var internalCode = Guid.NewGuid();
        var request = new CreatePropertyRequest
        {
            Name = "Casa D",
            Address = "Calle 4",
            Price = 4000M,
            InternalCode = internalCode,
            Year = 2024,
            IdOwner = 2,
            File = "data:image/png;base64,BBB..."
        };

        Property? capturedProperty = null;
        _propRepo.Setup(r => r.AddAsync(It.IsAny<Property>()))
                 .Callback<Property>(p => capturedProperty = p)
                 .Returns(Task.CompletedTask);

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Casa D"));
        Assert.That(result.Address, Is.EqualTo("Calle 4"));
        Assert.That(result.Price, Is.EqualTo(4000M));
        Assert.That(result.Year, Is.EqualTo(2024));
        Assert.That(result.IdOwner, Is.EqualTo(2));
        
        Assert.That(capturedProperty, Is.Not.Null);
        Assert.That(capturedProperty!.CodeInternal, Is.EqualTo(internalCode.ToString()));
        
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Once);
    }

    [Test]
    public async Task CreateProperty_WhenInternalCodeIsEmpty_GeneratesNewGuid()
    {
        var request = new CreatePropertyRequest
        {
            Name = "Casa E",
            Address = "Calle 5",
            Price = 5000M,
            InternalCode = Guid.Empty, 
            Year = 2024,
            IdOwner = 1,
            File = ""
        };

        Property? capturedProperty = null;
        _propRepo.Setup(r => r.AddAsync(It.IsAny<Property>()))
                 .Callback<Property>(p => capturedProperty = p)
                 .Returns(Task.CompletedTask);

        var handler = new CreatePropertyHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(capturedProperty, Is.Not.Null);
        Assert.That(capturedProperty!.CodeInternal, Is.Not.Null);
        Assert.That(capturedProperty.CodeInternal, Is.Not.Empty);
        Assert.That(Guid.TryParse(capturedProperty.CodeInternal, out var parsedGuid), Is.True);
        Assert.That(parsedGuid, Is.Not.EqualTo(Guid.Empty));
        _propRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
    }
}
