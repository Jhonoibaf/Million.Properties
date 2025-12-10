using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.Properties.API.Middlewares;
using Million.Properties.Application.DTOs;
using Million.Properties.Application.Features.Properties.Commands.CreateProperty;
using Million.Properties.Application.Features.Properties.Commands.UpdateProperty;
using Million.Properties.Application.Features.Properties.Commands.UpdatePropertyPrice;
using Million.Properties.Application.Features.Properties.Queries.GetAllProperties;
using Million.Properties.Application.Features.Properties.Queries.GetPropertyById;
using Million.Properties.Domain.Entities.Request.Properties;

namespace Million.Properties.API.Controllers.V1;

[ApiController]
[ApiKey]
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
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll(
        [FromQuery] string? name,
        [FromQuery] string? address,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var request = new GetAllPropertiesRequest
        {
            Name = name,
            Address = address,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };

        var query = new GetAllPropertiesQuery(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("UpdateProperty")]
    public async Task<IActionResult> UpdateProperty([FromBody] UpdatePropertyRequest request)
    {
        var command = new UpdatePropertyCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("UpdatePropertyPriceById/{id}/{price}")]
    public async Task<IActionResult> UpdatePropertyPriceById(int id, decimal price)
    {
        var request = new UpdatePropertyPriceByIdCommandRequest(id, price);
        var command = new UpdatePropertyPriceByIdCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
