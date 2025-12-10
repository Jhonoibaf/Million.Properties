using MediatR;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities.Request.Properties;

namespace Million.Properties.Application.Features.Properties.Commands.UpdatePropertyPrice;

public class UpdatePropertyPriceByIdCommand(UpdatePropertyPriceByIdCommandRequest request)
    : IRequest<PropertyDto>
{

    public UpdatePropertyPriceByIdCommandRequest Request { get; set; } = request;
}