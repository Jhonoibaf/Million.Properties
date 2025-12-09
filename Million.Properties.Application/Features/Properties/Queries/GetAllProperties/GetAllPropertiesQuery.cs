using MediatR;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities.Request;

namespace Million.Properties.Application.Features.Properties.Queries.GetAllProperties;

public class GetAllPropertiesQuery(GetAllPropertiesRequest request)
       : IRequest<IEnumerable<PropertyDto>>
{
    public GetAllPropertiesRequest Request { get; set; } = request;
}

