using AutoMapper;
using MediatR;
using Million.Properties.Application.Contracts;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Features.Properties.Commands.UpdateProperty;

public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, UpdatePropertyDto>
{
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePropertyHandler(IPropertyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UpdatePropertyDto> Handle(UpdatePropertyCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var propertyToUpdate = await _repository.GetByIdAsync(request.IdProperty);

        if (propertyToUpdate is null)
            throw new Exception($"Property with Id {request.IdProperty} was not found.");

        propertyToUpdate.Name = request.Name??propertyToUpdate.Name;
        propertyToUpdate.Address =request.Address ?? propertyToUpdate.Address;
        propertyToUpdate.Price = request.Price <= 0 ? propertyToUpdate.Price : request.Price;
        propertyToUpdate.CodeInternal = request.InternalCode == Guid.Empty ? propertyToUpdate.CodeInternal : request.InternalCode.ToString();
        propertyToUpdate.Year = request.Year <= 0 ? propertyToUpdate.Year : request.Year;
        propertyToUpdate.IdOwner = request.IdOwner ?? propertyToUpdate.IdOwner;

        await _repository.UpdateAsync(propertyToUpdate);

        return _mapper.Map<UpdatePropertyDto>(propertyToUpdate);
    }
}
