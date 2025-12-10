using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;

namespace Million.Properties.Application.Features.Properties.Queries.GetAllProperties;

public class GetAllPropertiesHandler(
        IPropertyRepository repository,
        IPropertyImageRepository imageRepo,
        IMapper mapper
    ) : IRequestHandler<GetAllPropertiesQuery, IEnumerable<PropertyDto>>
{
    private readonly IPropertyRepository _propertyRepository = repository;
    private readonly IPropertyImageRepository _imageRepo = imageRepo;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<PropertyDto>> Handle(GetAllPropertiesQuery query, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetAllWithFiltersAsync(
           query.Request.Name,
           query.Request.Address,
           query.Request.MinPrice,
           query.Request.MaxPrice
       );

        var dtos = _mapper.Map<IEnumerable<PropertyDto>>(properties).ToList();

        var ids = dtos.Select(p => p.IdProperty);
        var imagesDict = await _imageRepo.GetAllImagesByPropertyIdsAsync(ids);

        foreach (var dto in dtos)
        {
            if (imagesDict.TryGetValue(dto.IdProperty, out var imgs))
            {
                dto.Images = _mapper.Map<List<PropertyImageDto>>(imgs);
            }
        }
        return dtos;
    }
}
