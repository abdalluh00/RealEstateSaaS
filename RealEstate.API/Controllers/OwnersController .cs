using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Owners.Commands.CreateOwner;
using RealEstate.Application.Features.Owners.Commands.DeleteOwner;
using RealEstate.Application.Features.Owners.Commands.UpdateOwner;
using RealEstate.Application.Features.Owners.Queries.GetOwnerById;
using RealEstate.Application.Features.Owners.Queries.GetOwners;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnersController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOwnersQuery());
            return Ok(result);
        }

        [HttpGet("detail/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOwnerByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOwnerCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOwnerCommand command)
        {
            if (id != command.Id) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteOwnerCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
