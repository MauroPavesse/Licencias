using Licencias.Application.Entities.Extras.Create;
using Licencias.Application.Entities.Extras.Delete;
using Licencias.Application.Entities.Extras.Search;
using Licencias.Application.Entities.Extras.Update;
using Licencias.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Licencias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExtraController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] SearchCommand command)
        {
            var result = await _mediator.Send(new ExtraSearchCommand(command));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExtraCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ExtraUpdateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new ExtraDeleteCommand(id));
            return Ok(result);
        }
    }
}
