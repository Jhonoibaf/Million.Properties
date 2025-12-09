using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.Properties.Application.Features.Properties.Commands.CreateProperty;

namespace Million.Properties.API.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class ImagesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("AddImage")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}