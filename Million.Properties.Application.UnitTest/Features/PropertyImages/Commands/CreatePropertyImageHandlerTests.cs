using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.Features.PropertyImages.Commands;
using Million.Properties.Application.Mappings;
using Million.Properties.Domain.Entities;
using Million.Properties.Domain.Entities.Request.PropertyImage;
using Moq;
using NUnit.Framework;

namespace Million.Properties.Application.UnitTest.Features.PropertyImages.Commands;

[TestFixture]
public class CreatePropertyImageHandlerTests
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

        _imgRepo.Setup(r => r.AddAsync(It.IsAny<PropertyImage>()))
                .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task CreatePropertyImage_WhenPropertyExists_CreatesImageSuccessfully()
    {
        var propertyId = 1;
        var property = new Property
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = Guid.NewGuid().ToString(),
            Year = 2024
        };

        _propRepo.Setup(r => r.GetByIdAsync(propertyId))
                 .ReturnsAsync(property);

        PropertyImage? capturedImage = null;
        _imgRepo.Setup(r => r.AddAsync(It.IsAny<PropertyImage>()))
                .Callback<PropertyImage>(img => capturedImage = img)
                .Returns(Task.CompletedTask);

        var request = new CreatePropertyImageRequest
        {
            IdProperty = propertyId,
            File = "test-image.jpg"
        };

        var handler = new CreatePropertyImageHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyImageCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdProperty, Is.EqualTo(propertyId));
        Assert.That(result.File, Is.EqualTo("test-image.jpg"));
        Assert.That(result.Enabled, Is.True);
        
        Assert.That(capturedImage, Is.Not.Null);
        Assert.That(capturedImage!.IdProperty, Is.EqualTo(propertyId));
        Assert.That(capturedImage.File, Is.EqualTo("test-image.jpg"));
        Assert.That(capturedImage.Enabled, Is.True);
        
        _propRepo.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Once);
    }

    [Test]
    public void CreatePropertyImage_WhenPropertyDoesNotExist_ThrowsKeyNotFoundException()
    {
        var propertyId = 999;

        _propRepo.Setup(r => r.GetByIdAsync(propertyId))
                 .ReturnsAsync((Property?)null);

        var request = new CreatePropertyImageRequest
        {
            IdProperty = propertyId,
            File = "test-image.jpg"
        };

        var handler = new CreatePropertyImageHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await handler.Handle(new CreatePropertyImageCommand(request), CancellationToken.None));

        _propRepo.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        _imgRepo.Verify(r => r.AddAsync(It.IsAny<PropertyImage>()), Times.Never);
    }

    [Test]
    public async Task CreatePropertyImage_WithEmptyFile_CreatesImageWithEmptyFile()
    {
        var propertyId = 1;
        var property = new Property
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = Guid.NewGuid().ToString(),
            Year = 2024
        };

        _propRepo.Setup(r => r.GetByIdAsync(propertyId))
                 .ReturnsAsync(property);

        var request = new CreatePropertyImageRequest
        {
            IdProperty = propertyId,
            File = string.Empty
        };

        var handler = new CreatePropertyImageHandler(_propRepo.Object, _mapper, _imgRepo.Object, _unitOfWork.Object);
        var result = await handler.Handle(new CreatePropertyImageCommand(request), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.File, Is.Empty);
        Assert.That(result.Enabled, Is.True);
    }
}
