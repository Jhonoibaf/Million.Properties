using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyHandler(
    IPropertyRepository repository, 
    IMapper mapper, 
    IPropertyImageRepository imageRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePropertyCommand, CreatePropertyDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreatePropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        if (!request.Request.InternalCode.HasValue || request.Request.InternalCode.Value == Guid.Empty)
        {
            request.Request.InternalCode = Guid.NewGuid();
        }

        var entity = _mapper.Map<Property>(request.Request);
        await _unitOfWork.PropertyRepository.AddAsync(entity);

        if (!string.IsNullOrWhiteSpace(request.Request.File))
        {
            var img = new PropertyImage
            {
                IdProperty = entity.IdProperty,
                File = request.Request.File,
                Enabled = true
            };
            await _unitOfWork.PropertyImageRepository.AddAsync(img);
        }

        return _mapper.Map<CreatePropertyDto>(entity);
    }
}
