using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.Properties.API.Middlewares;
using Million.Properties.Application.Features.PropertyImages.Commands;
using Million.Properties.Domain.Entities.Request.PropertyImage;

namespace Million.Properties.API.Controllers.V1;

[ApiController]
[ApiKey]
[Route("api/V1/[controller]")]
public class ImagesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("CreateImage")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyImageRequest request)
    {
        var command = new CreatePropertyImageCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}