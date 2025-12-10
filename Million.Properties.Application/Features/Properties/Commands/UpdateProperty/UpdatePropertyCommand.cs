using MediatR;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities.Request.Properties;

namespace Million.Properties.Application.Features.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommand(UpdatePropertyRequest request)
    : IRequest<UpdatePropertyDto>
{
    public UpdatePropertyRequest Request { get; set; } = request;
}
