using MediatR;
using Million.Properties.Application.DTOs;

namespace Million.Properties.Application.Features.Properties.Queries.GetPropertyById;

public class GetPropertyByIdQuery(int id) : IRequest<PropertyDto?>
{
    public int Id { get; set; } = id;
}   