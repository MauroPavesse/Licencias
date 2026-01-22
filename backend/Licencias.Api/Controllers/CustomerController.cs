using Licencias.Application.Entities.Customers.Create;
using Licencias.Application.Entities.Customers.Delete;
using Licencias.Application.Entities.Customers.Search;
using Licencias.Application.Entities.Customers.Update;
using Licencias.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Licencias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] SearchCommand command)
        {
            var result = await _mediator.Send(new CustomerSearchCommand(command));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new CustomerDeleteCommand(id));
            return Ok(result);
        }
    }
}
