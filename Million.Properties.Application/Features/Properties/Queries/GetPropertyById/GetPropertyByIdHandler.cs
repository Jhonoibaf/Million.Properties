using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;

namespace Million.Properties.Application.Features.Properties.Queries.GetPropertyById;

public class GetPropertyByIdHandler(
    IPropertyRepository propertyRepository,
    IPropertyImageRepository imageRepository,
    IMapper mapper)
    : IRequestHandler<GetPropertyByIdQuery, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IPropertyImageRepository _imageRepository = imageRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PropertyDto?> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        if (property == null)
            return null;

        var dtoProperty = _mapper.Map<PropertyDto>(property);

        var propertyImages = await _imageRepository.GetByPropertyIdAsync(property.IdProperty);
        var validImages = propertyImages
            .Where(img => img.Enabled && !string.IsNullOrWhiteSpace(img.File))
            .OrderBy(img => img.IdPropertyImage)
            .ToList();

        dtoProperty.Images = validImages;

        return dtoProperty;
    }
}
