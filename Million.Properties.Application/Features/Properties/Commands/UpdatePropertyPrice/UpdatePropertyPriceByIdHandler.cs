using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;

namespace Million.Properties.Application.Features.Properties.Commands.UpdatePropertyPrice;

public class UpdatePropertyPriceByIdHandler : IRequestHandler<UpdatePropertyPriceByIdCommand, PropertyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePropertyPriceByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PropertyDto> Handle(UpdatePropertyPriceByIdCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var property = await _unitOfWork.PropertyRepository.GetByIdAsync(request.IdProperty);

        if (property is null)
            throw new Exception($"Property with Id {request.IdProperty} was not found.");

        property.Price = request.NewPrice;

        await _unitOfWork.PropertyRepository.UpdateAsync(property);

        return _mapper.Map<PropertyDto>(property);
    }
}

