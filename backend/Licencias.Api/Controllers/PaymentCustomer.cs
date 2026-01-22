using Licencias.Application.Entities.Payments.Create;
using Licencias.Application.Entities.Payments.Delete;
using Licencias.Application.Entities.Payments.Search;
using Licencias.Application.Entities.Payments.Update;
using Licencias.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Licencias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentPayment : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentPayment(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] SearchCommand command)
        {
            var result = await _mediator.Send(new PaymentSearchCommand(command));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PaymentUpdateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new PaymentDeleteCommand(id));
            return Ok(result);
        }
    }
}
