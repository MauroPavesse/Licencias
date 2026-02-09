using Licencias.Application.Entities.Subscriptions.Create;
using Licencias.Application.Entities.Subscriptions.Delete;
using Licencias.Application.Entities.Subscriptions.Search;
using Licencias.Application.Entities.Subscriptions.Update;
using Licencias.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Licencias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchCommand command)
        {
            var result = await _mediator.Send(new SubscriptionSearchCommand(command));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubscriptionCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SubscriptionUpdateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new SubscriptionDeleteCommand(id));
            return Ok(result);
        }
    }
}
