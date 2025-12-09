using MediatR;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities.Request;


namespace Million.Properties.Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyCommand(CreatePropertyRequest request): IRequest<CreatePropertyDto>
{
    public CreatePropertyRequest Request { get; set; } = request;
}