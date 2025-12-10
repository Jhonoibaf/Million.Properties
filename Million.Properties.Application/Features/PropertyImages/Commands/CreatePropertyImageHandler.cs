using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Features.PropertyImages.Commands;

public class CreatePropertyImageHandler(
    IPropertyRepository repository,
    IMapper mapper,
    IPropertyImageRepository imageRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePropertyImageCommand, PropertyImageDto>
{
    private readonly IPropertyRepository _propertyRepository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

   
    public async Task<PropertyImageDto> Handle(CreatePropertyImageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var property = await _propertyRepository.GetByIdAsync(request.IdProperty);

        if (property == null)
            throw new KeyNotFoundException($"Property with ID {request.IdProperty} was not found.");

        var entity = new PropertyImage
        {
            IdProperty = request.IdProperty,
            File = request.File,
            Enabled = true,
            CreatedOn = DateTime.UtcNow
        };

        await _unitOfWork.PropertyImageRepository.AddAsync(entity);

        return new PropertyImageDto
        {
            IdPropertyImage = entity.IdPropertyImage,
            IdProperty = entity.IdProperty,
            File = entity.File,
            Enabled = entity.Enabled
        };
    }
}
