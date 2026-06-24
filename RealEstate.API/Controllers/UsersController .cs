using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Users.Commands.ChangePassword;
using RealEstate.Application.Features.Users.Commands.CreateUser;
using RealEstate.Application.Features.Users.Commands.DeleteUser;
using RealEstate.Application.Features.Users.Commands.UpdateUser;
using RealEstate.Application.Features.Users.Queries.GetUserById;
using RealEstate.Application.Features.Users.Queries.GetUsers;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
           // var companyId = Guid.Parse(User.FindFirst("CompanyId")!.Value);
            var result = await _mediator.Send(new GetUsersQuery());
            return Ok(result);
        }

        [HttpGet("detail/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}/change-password")]
        public async Task<IActionResult> ChangePassword(
            Guid id,
            [FromBody] ChangePasswordCommand command)
        {
            if (id != command.UserId) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
