using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.Properties.Application.DTOs;
using Million.Properties.Application.Features.Properties.Commands.CreateProperty;
using Million.Properties.Application.Features.Properties.Queries.GetAllProperties;
using Million.Properties.Application.Features.Properties.Queries.GetPropertyById;
using Million.Properties.Domain.Entities.Request;

namespace Million.Properties.API.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class PropertyController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("CreateProperty")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("GetProperty/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetPropertyByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("GetAllProperties")]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll([FromQuery] GetAllPropertiesRequest request)
    {
        var query = new GetAllPropertiesQuery(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    //update property
}
