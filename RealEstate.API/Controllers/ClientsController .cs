using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Clients.Commands.CreateClient;
using RealEstate.Application.Features.Clients.Commands.DeleteClient;
using RealEstate.Application.Features.Clients.Commands.UpdateClient;
using RealEstate.Application.Features.Clients.Queries.GetClientById;
using RealEstate.Application.Features.Clients.Queries.GetClients;

namespace RealEstate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator) => _mediator = mediator;

        //[HttpGet("{companyId:guid}")]
        //public async Task<IActionResult> GetAll(Guid companyId, [FromQuery] string? leadStatus)
        //{
        //    var result = await _mediator.Send(new GetClientsQuery(companyId, leadStatus));
        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] string? leadStatus)
        {
            var result = await _mediator.Send(new GetClientsQuery( leadStatus));
            return Ok(result);
        }

        [HttpGet("detail/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetClientByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientCommand command)
        {
            if (id != command.Id) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteClientCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
