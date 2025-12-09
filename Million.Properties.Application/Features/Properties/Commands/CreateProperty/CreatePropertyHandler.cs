using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyHandler(IPropertyRepository repository, IMapper mapper , IPropertyImageRepository imageRepository)
    : IRequestHandler<CreatePropertyCommand, CreatePropertyDto>
{
    private readonly IPropertyRepository _propertyRepository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IPropertyImageRepository _imageRepository = imageRepository;

    public async Task<CreatePropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Property>(request.Request);
            await repository.AddAsync(entity);

            if (!string.IsNullOrWhiteSpace(request.Request.File))
            {
                var img = new PropertyImage
                {
                    IdProperty = entity.IdProperty,
                    File = request.Request.File
                };
                await imageRepository.AddAsync(img);
            }

            return _mapper.Map<CreatePropertyDto>(entity);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while creating the property.", ex);
        }
    }
}
