using MediatR;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities.Request.PropertyImage;

namespace Million.Properties.Application.Features.PropertyImages.Commands;

public class CreatePropertyImageCommand(CreatePropertyImageRequest request)
    : IRequest<PropertyImageDto>
{
    public CreatePropertyImageRequest Request { get; set; } = request;
}